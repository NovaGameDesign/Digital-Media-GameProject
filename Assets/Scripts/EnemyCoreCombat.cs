using System;
using DigitalMedia.Combat;
using DigitalMedia.Core;
using DigitalMedia.Interfaces;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalMedia
{
    public class EnemyCoreCombat : CoreCharacter
    {
        #region Behavior Tree Related Functionality. 
        
        [SerializeField] private bool isAnAgent;
        private NavMeshAgent agent;
        public BehaviourTreeInstance behaviourTreeInstance;
        [SerializeField] private Transform player;

        private BlackboardKey<Vector2> keyRef;
        private BlackboardKey<GameObject> playerKeyRef;
        
        #endregion
        
        private Collider2D[] overlapping;

        [SerializeField] protected LayerMask layersToCheck;

        private int currentAttackIndex = 0;
        public bool parrying;
        protected bool blocking;
        private GameObject targetThatParried;
        
        //Unlike the Player character's animation names, these are intended to be used across the base version of all that inherits from these. In other words, these should work on both the player and enemy. 

        #region Attack Related Functionality

        private StatsComponent stats;
        void Start()
        {
            _animator = GetComponent<Animator>();

            stats = GetComponent<StatsComponent>();

            InitateStateChange(State.Idle);
            
            playerKeyRef = behaviourTreeInstance.FindBlackboardKey<GameObject>("Player GameObject");
            if (isAnAgent)
            {
                agent = GetComponent<NavMeshAgent>();
                agent.updateRotation = false;
                agent.updateUpAxis = false;
                keyRef = behaviourTreeInstance.FindBlackboardKey<Vector2>("Player Position");
                playerKeyRef.value = player.gameObject;
            }
        }

        private void FixedUpdate()
        {
            if(isAnAgent) keyRef.value = player.transform.position;
            
        }
        
        public virtual void TryToAttack(string desiredAttack)
        {
            // Debug.Log("Attacked");

            if (currentState != State.Idle && currentState != State.Airborne && !canInterruptState) //Check what state the player is in. Generally they'd need to be in one of the aforementioned states.
            {
                return;
            }

            InitateStateChange(State.Attacking);

            switch (desiredAttack)
            {
                case "BasicAttackCombo":
                {
                    _animator.Play("Attack_One");
                    currentAttackIndex++;
                    return;
                }
                case "Two Hit":
                {
                    _animator.Play("Attack_Two"); // Update these once we have the other anims. 
                    currentAttackIndex++;
                    return;
                } 
                case "Three Hit":
                {
                    _animator.Play("Attack_Three");
                    currentAttackIndex = 0;
                    return;
                }
                case "Attack_Combo":
                {
                    _animator.Play("Attack_Combo"); // Update these once we have the other anims. 
                    currentAttackIndex++;
                    return;
                }
                
            }
        }

        /// <summary>
        /// Handle the attack functionality related to hit boxes and dealing damage. In here we spawn colliders and then do a variety of checks. 
        /// </summary>
        /// <param name="rangeToUse"></param>
        public virtual void HandleBasicAttack(int rangeToUse = 0)
        {
            //Theoretically if the enemy has multiple attacks, and thus attack sizes, we'd change the index from 0 -> whatever is associated  with that attack. 
            if (transform.rotation.y > 0)
            {
                //Holy crap this was a pain to setup. For some reason the transform didn't want to work if it was == 180 even though that is the exact value :( 
                Vector2 otherSide = new Vector2(data.CombatData.weaponData.weaponOffset[rangeToUse].x * -1, data.CombatData.weaponData.weaponOffset[rangeToUse].y); 
                overlapping = Physics2D.OverlapCircleAll(transform.position + (Vector3)otherSide, data.CombatData.weaponData.weaponRange[rangeToUse].x, layersToCheck);
                //Debug.Log("Changed the side"+ otherSide);
            }
            else if (transform.rotation.y == 0)
            {
                overlapping = Physics2D.OverlapCircleAll(transform.position + (Vector3)data.CombatData.weaponData.weaponOffset[rangeToUse], data.CombatData.weaponData.weaponRange[rangeToUse].x, layersToCheck);
            }


            foreach (var hit in overlapping)
            {
                if (hit.transform.root == transform) continue;
                
                // If the target is deathblowing we don't want to deal any damage to them. 
                if (hit.GetComponent<CombatSystem>()?.currentState == State.Deathblowing)
                {
                    overlapping = null;
                    return; 
                }

                if (hit.GetComponent<CombatSystem>() != null && hit.GetComponent<CombatSystem>().parrying)
                {
                   
                    hit.GetComponent<ICombatCommunication>().DidParry(); //Calling on the target
                    WasParried();
                    if (stats.vitality < 0)
                    {
                        InitateStateChange(State.Staggered);
                        _animator.Play("Staggered");
                        hit.GetComponent<CombatSystem>().deathblowTarget = this.gameObject;
                        targetThatParried = hit.gameObject;
                    }
                    overlapping = null;
                    return;
                }
                
                if (hit.GetComponent<IDamageable>() != null)
                {
                    var damage = data.CombatData.weaponData.innateWeaponDamage * data.CombatData.attackPower;
                    hit.GetComponent<IDamageable>().DealDamage(damage, this.gameObject, true);
                }
            }

            overlapping = null;
        }
        
        public void EndAttackSequence()
        {
            InitateStateChange(State.Idle);
            currentAttackIndex = 0;
            _animator.Play("Idle");
        }

        public void DisableDeathblow()
        {
            InitateStateChange(State.Idle);
            _animator.Play("Idle");
            targetThatParried.GetComponent<CombatSystem>().deathblowTarget = null;
            stats.vitality = data.BasicData.maxVitality / 4;
        }

        #endregion


        #region Parry Related Functionality

        private void TryToBlock(bool stop = false)
        {
            if (stop)
            {
                //Debug.Log("Stopped holding right click;");
                InitateStateChange(State.Idle);
                _animator.Play("Player_Block_End");
            }
            else if (!stop)
            {
                if (currentState != State.Idle && !canInterruptState) return;

                InitateStateChange(State.Blocking);

                _animator.Play("Block");
                parrying = true;
            }
        }

        public void StartStopParrying(string shouldParry)
        {
            parrying = shouldParry == "true" ? true : false;
        }

        public void WasParried()
        {
            //Implement animation parry reaction and state changes. 
            _animator.Play("Parried");
            stats.DealVitalityDamage(15);
            return;
        }

        public void DidParry() 
        {
            _animator.Play("Parry");

            GameObject sparks = ObjectPool.SharedInstance.GetPooledObject();
            if (sparks != null)
            {
                Transform sparksSpawnLocation = this.transform.Find("ParrySparksLocation").transform;
                sparks.transform.position = sparksSpawnLocation.position;
                sparks.transform.rotation = sparksSpawnLocation.rotation;
                sparks.SetActive(true);
            }
        }

        #endregion

        //Used to visualize the range and position of attacks. Each attack can be configured from their data scriptable object. 
        public void OnDrawGizmosSelected()
        {
            if (data == null)
                return;

            if (data.CombatData.weaponData.ShowAttackDebug)
            {
                switch (data.CombatData.weaponData.ColliderType)
                {
                    case ColliderType.box:
                    {

                        Gizmos.DrawWireCube(
                            transform.position +
                            (Vector3)data.CombatData.weaponData.weaponOffset[data.CombatData.weaponData.showWhat],
                            (Vector3)data.CombatData.weaponData.weaponRange[data.CombatData.weaponData.showWhat]);

                        break;
                    }
                    case ColliderType.circle:
                    {
                        for (int i = 0; i < data.CombatData.weaponData.weaponOffset.Length; i++)
                        {
                            Gizmos.DrawWireSphere(
                                transform.position + (Vector3)data.CombatData.weaponData.weaponOffset[i],
                                data.CombatData.weaponData.weaponRange[i].x);
                        }

                        break;
                    }
                    case ColliderType.capsule:
                    {
                        break;
                    }
                }

            }

            if (data.CombatData.ShowParryDebug)
            {
                Gizmos.DrawWireCube(transform.position + (Vector3)data.CombatData.parryOffset,
                    (Vector3)data.CombatData.parryRange);
            }
        }
    }
}

