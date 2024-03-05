using DigitalMedia.Combat.Abilities;
using DigitalMedia.Core;
using DigitalMedia.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DigitalMedia.Combat
{
    public class CombatSystem : CoreCharacter, ICombatCommunication
    {
        public AbilityHolder abilities;
        
        
        #region Input

        private PlayerInput _playerInput;
        private InputAction attack;
        private InputAction block;
        
        #endregion

        private Collider2D[] overlapping;

        [SerializeField] protected LayerMask layersToCheck;

        private int currentAttackIndex = 0;
        public bool parrying;
        public GameObject deathblowTarget = null;
        [SerializeField] protected GameObject deathblowAirSlash;
        [SerializeField] private GameObject blood;
        
        [System.NonSerialized] public bool blocking;

        #region Animation

        private const string ANIM_IDLE = "Idle";
        private const string ANIM_ATTACK_ONE = "Attack_One";
        private const string ANIM_ATTACK_TWO = "Attack_Two";
        private const string ANIM_ATTACK_THREE = "Attack_Three";
        private const string ANIM_BLOCK = "Player_Block_Start";

        #endregion

        //Unlike the Player character's animation names, these are intended to be used across the base version of all that inherits from these. In other words, these should work on both the player and enemy. 

        private void Start()
        {
            InitateStateChange(State.Idle);
            //Input
            _playerInput = GetComponent<PlayerInput>();
            attack = _playerInput.actions["Attack"];
            attack.performed += TryToAttack;
            block = _playerInput.actions["Block"];
            block.performed += TryToBlock;
            block.canceled += TryToBlock;

            _animator = GetComponent<Animator>();
        }

        #region Attack Related Functionality

        public virtual void TryToAttack(InputAction.CallbackContext context)
        {
            // Debug.Log("Attacked");
            if (deathblowTarget != null)
            {
               Deathblow();
               return;
            }
            
            if (currentState != State.Idle && currentState != State.Moving && currentState != State.Airborne && !canInterruptState) //Check what state the player is in. Generally they'd need to be in one of the aforementioned states.
            {
                return;
            }

            InitateStateChange(State.Attacking);
            //Debug.Log($"The player's new state is: {currentState}");
            /*if (currentState == State.Airborne)
            {
                AirborneAttack();
                return;
            }*/
         
            switch (currentAttackIndex)
            {
                case 0:
                {
                    Debug.Log("Launched attack one");
                    _animator.Play(ANIM_ATTACK_ONE);
                    currentAttackIndex++;
                    return;
                }
                case 1:
                {
                    _animator.Play(ANIM_ATTACK_TWO); // Update these once we have the other anims. 
                    currentAttackIndex++;
                    return;
                }
                case 2:
                {
                    _animator.Play(ANIM_ATTACK_THREE);
                    currentAttackIndex = 0;
                    return;
                }
            }
        }

        public virtual void HandleBasicAttack()
        {
            //Theoretically if the enemy has multiple attacks, and thus attack sizes, we'd change the index from 0 -> whatever is associated  with that attack. 
            if (transform.rotation.y > 0)
            {
                //Holy crap this was a pain to setup. For some reason the transform didn't want to work if it was == 180 even though that is the exact value :( 
                Vector2 otherSide = new Vector2(data.CombatData.weaponData.weaponOffset[currentAttackIndex].x * -1,
                    data.CombatData.weaponData.weaponOffset[currentAttackIndex].y);
                overlapping = Physics2D.OverlapBoxAll(transform.position + (Vector3)otherSide,
                    (Vector3)data.CombatData.weaponData.weaponRange[currentAttackIndex],
                    layersToCheck);
                //Debug.Log("Changed the side"+ otherSide);
            }
            else if (transform.rotation.y == 0)
            {
                overlapping = Physics2D.OverlapBoxAll(
                    transform.position + (Vector3)data.CombatData.weaponData.weaponOffset[currentAttackIndex],
                    (Vector3)data.CombatData.weaponData.weaponRange[currentAttackIndex],
                    layersToCheck);
            }


            foreach (var hit in overlapping)
            {
                if (hit.transform.root == transform) continue;

                if (hit.GetComponent<IDamageable>() != null)
                {
                    var damage = data.CombatData.weaponData.innateWeaponDamage * data.CombatData.attackPower;
                    hit.GetComponent<IDamageable>().DealDamage(damage, this.gameObject, true);
                }
            }

            overlapping = null;

        }

        private void AirborneAttack()
        {
            _animator.Play("Attack_Airborne");

            //DO some checks to see if we hit anything
        }

        public void EndAttackSequence()
        {
            InitateStateChange(State.Idle);
            currentAttackIndex = 0;
            _animator.Play(ANIM_IDLE);
        }

        #endregion


        #region Parry Related Functionality

        private void TryToBlock(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                //Debug.Log("Stopped holding right click;");
                InitateStateChange(State.Idle);
                _animator.Play("Player_Block_End");
                blocking = false;
            }
            else if (context.performed)
            {
                if (currentState != State.Idle && !canInterruptState) return;

                InitateStateChange(State.Blocking);

                _animator.Play(ANIM_BLOCK);
                blocking = true;
            }
        }

        public void StartStopParrying(string shouldParry)
        {
            parrying = shouldParry == "true" ? true : false;;
        }

        public void WasParried()
        {
            throw new System.NotImplementedException();
        }

        public void DidParry()
        {
            _animator.Play("Player_Parry");
            parrying = false; 
            
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

        #region Deathblow
        
        public void Deathblow()
        {
            InitateStateChange(State.Deathblowing);
            /*transform.GetComponent<Rigidbody2D>().simulated = false; The idea here is to maybe let the player "teleport" to their destination and do some sort of flash step quick attack deathblow. Idrk I'll probably do it when I have a bit more time to do afterimages for the player teleporting and stuff.
            transform.position = deathblowTarget.transform.Find("Deathblow Position").position;*/
           
            _animator.Play("Deathblow");
        }

        public void EndDeathblowSequence()
        {
            InitateStateChange(State.Idle);
            transform.GetComponent<Rigidbody2D>().simulated = true;
            //Other stuff
        }
        
        public void SpawnDeathblowSlash()
        {
            //Play animations if we end up having one, otherwise just destroy the target and spawn the slash
            Instantiate(deathblowAirSlash, deathblowAirSlash.transform);
            Instantiate(blood, deathblowTarget.transform);
            
            Destroy(deathblowTarget.gameObject);
            deathblowTarget = null;
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
