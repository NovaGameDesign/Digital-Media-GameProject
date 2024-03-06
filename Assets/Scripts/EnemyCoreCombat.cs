using System;
using System.Collections;
using AYellowpaper.SerializedCollections;
using DigitalMedia.Combat;
using DigitalMedia.Combat.Abilities;
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

        [NonSerialized] public StatsComponent stats;

        #region Ability System Variables and enums 
        
        [SerializedDictionary("Attack Name", "Attack Scriptable Object Ref")]
        public SerializedDictionary<string, AbilityBase> abilities;

        private AbilityBase currentAbility;
        private AbilityBase oldAbility; 
        
        #endregion
        
      
        void Start()
        {
            _animator = GetComponent<Animator>();
            _audioPlayer = GetComponent<AudioSource>();

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
            TriggerAbility(desiredAttack); // In the event we didn't update an old call. 
           
            /*if (currentState != State.Idle && currentState != State.Airborne && !canInterruptState) //Check what state the player is in. Generally they'd need to be in one of the aforementioned states.
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
                }*/

            }
        
        /// <summary>
        /// Conclusion I made before heading to bed:
        /// Abilities themselves should (if possible) only handle additional functionality, ie. moving the player forward, teleporting, etc.
        /// This mono behavior and others like it should try (as much as possible) to simply interface with and interpret animation events.
        /// This may mean canceling an ability or changing states, but it should be things the scriptable object can't do.
        /// What I need to finish writing is how certain events are executed (ie. damage being dealt) while using our new data system from the Ability. Furthermore, in those events we need to actually activate certain things in the Ability such as knockback. 
        ///
        /// Furthermore, some functionality that could be in the ability will stay in this class or its children as it never changes, such as the collider check. The data for such checks will, however, be in the ability.
        /// In the event of damage being dealt, we instead treat the ability like a pure data scriptable object (SO) and only if necessary call additional functions.
        ///
        /// IF I implement this properly it does a couple things:
        /// 1. More robust data for different abilities -- self explanatory but now I won't have to have seven billion audio clips on one monobehavior.
        /// 2. Reducing hard-coded functionality -- Instead of having everything hard coded we can simply pass whatever ability reference we want and theoretically it will execute its own desired code and pass data if needed.
        /// 3. Reduce amount of code in this or other scripts -- Because some of our functionality is now directly on the data container (which is more or less what the AbilityBase class is) this script gets much smaller but also more robust as it is far more versatile.
        /// 4. Multi-action changes -- Instead of having a few shared actions across multiple scripts that each need to be individually updated, we can share a single Ability SO across them which updates all rather than one.
        /// 4.1. A theoretical limitation of this is the event in which we want to have the same attack but different ranges, stamina, damage, etc. In this case we simply need to make another SO and change the data, which becomes even more simple!
        ///
        /// My plans for the morning include settings up the audio system to actually do what it needs to, and also finishing this class setup.
        /// </summary>
        /// <param name="desiredAbility"></param>
        public void TriggerAbility(string desiredAbility)
        {
            abilities.TryGetValue(desiredAbility, out currentAbility);
            //Debug.Log($"The ability we found was {currentAbility}.");
            //Debug.Log($"Tried to trigger an ability, we found: {currentAbility}");
            
            if (currentAbility.currentAbilityState != AbilityBase.AbilityStates.ReadyToActivate)
                return;
            if (!CharacterIsInAllowedState())
                return;

            //play animation 
            _animator.Play(currentAbility.animToPlay.name);
            InitateStateChange(State.Attacking);
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleAbilityUsage()
        {
            //currentAbility.currentAbilityState = AbilityBase.AbilityStates.Using;
            
            //Functionality stored in the Scriptable Object that performs the ability's logic. 
            //While most of these will likely just call HandleBasicAttack, in some cases we might not do this or have additional logic performed either on the SO or here. 
            currentAbility.Activate(this.gameObject);
        }

        public void ActivateAbilityEffects()
        {
            currentAbility.ActivateAbilityEffects(this.gameObject);
        }
        public void EndAbilityUsage()
        {
            //Swap the ability's state to cooldown
            currentAbility.currentAbilityState = AbilityBase.AbilityStates.OnCooldown;
            
            InitateStateChange(State.Idle);
            _animator.Play("Idle");
            
            if (currentAbility.hasCooldown)
            {
                StartCoroutine(HandleCooldown_CO());
            }
        }
        
        private IEnumerator HandleCooldown_CO()
        {
            yield return new WaitForSeconds(currentAbility.cooldown);

            currentAbility.currentAbilityState = AbilityBase.AbilityStates.ReadyToActivate;
        }
        
        public bool CharacterIsInAllowedState()
        {
            return currentAbility.allowedUsageStates.Contains(currentState);
        }
        
        /// <summary>
        /// Handle the attack functionality related to hit boxes and dealing damage. In here we spawn colliders and then do a variety of checks. 
        /// </summary>
        /// <param name="rangeToUse"></param>
        public virtual void HandleBasicAttack(Vector2 weaponOffset, Vector2 weaponRange)
        {
            //Theoretically if the enemy has multiple attacks, and thus attack sizes, we'd change the index from 0 -> whatever is associated  with that attack. 
            if (transform.rotation.y > 0)
            {
                //Holy crap this was a pain to setup. For some reason the transform didn't want to work if it was == 180 even though that is the exact value :( 
                Vector2 otherSide = new Vector2(weaponOffset.x * -1, weaponOffset.y); 
                overlapping = Physics2D.OverlapCircleAll(transform.position + (Vector3)otherSide, weaponRange.x, layersToCheck);
                //Debug.Log("Changed the side"+ otherSide);
            }
            else if (transform.rotation.y == 0)
            {
                overlapping = Physics2D.OverlapCircleAll(transform.position + (Vector3)weaponOffset, weaponRange.x, layersToCheck);
            }


            foreach (var hit in overlapping)
            {
                if (hit.transform.root == transform) continue; //Checks if it hit self. 
                
                // If the target is deathblowing we don't want to deal any damage to them. 
                if (hit.GetComponent<CombatSystem>()?.currentState == State.Deathblowing) //Checks if the target is deathblow -- in other words whether we should deal damage.  
                {
                    overlapping = null;
                    return; 
                }

                if (hit.GetComponent<CombatSystem>() != null && hit.GetComponent<CombatSystem>().parrying) // Checks if the target is parrying. 
                {
                    //In this call we need to be able to specify what KIND of attack was parried, range, melee, strong, etc. 
                    hit.GetComponent<ICombatCommunication>().DidParry(); //Calling on the target
                    //Add functionality to change the parry reaction based on the given attack type. Also add a paramater for if the attack should even be interrupted or not. 
                    WasParried();
                    //Check if the ability has any additional functionality it needs to execute after being parried. 
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
                    _audioPlayer.PlayOneShot(currentAbility.sfxHit);
                    
                }
            }

            overlapping = null;
        }
        
        public void EndAttackSequence()
        {
            InitateStateChange(State.Idle);
            /*currentAttackIndex = 0;*/
            _animator.Play("Idle");
        }

        /// <summary>
        /// Called via the animation associated with the deathblow. 
        /// </summary>
        public void DisableDeathblow()
        {
            InitateStateChange(State.Idle);
            _animator.Play("Idle");
            targetThatParried.GetComponent<CombatSystem>().deathblowTarget = null;
            stats.vitality = data.BasicData.maxVitality / 4;
        }

        #endregion


        #region Parry Related Functionality

        public void WasParried()
        {
            //Implement animation parry reaction and state changes. 
            _animator.Play("Parried");
            stats.DealVitalityDamage(15);
            return;
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

