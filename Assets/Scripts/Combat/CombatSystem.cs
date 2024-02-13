using DigitalMedia.Core;
using DigitalMedia.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DigitalMedia.Combat
{
    public class CombatSystem : CoreCharacter
    {
        //Input Related
        private PlayerInput _playerInput;
        private InputAction attack;
        private InputAction block;

        protected Collider2D[] overlapping;

        [SerializeField] protected LayerMask layersToCheck;

        private int currentAttackIndex = 0;
        
        //Unlike the Player character's animation names, these are intended to be used across the base version of all that inherits from these. In other words, these should work on both the player and enemy. 
        private const string ANIM_IDLE = "Idle";
        private const string ANIM_ATTACK_ONE = "Attack_One";
        private const string ANIM_ATTACK_TWO = "Attack_Two";
        private const string ANIM_ATTACK_THREE = "Attack_Three";
        private const string ANIM_BLOCK = "Block";

        private void Start()
        {
            currentState = State.Idle;
            //Input
            _playerInput = GetComponent<PlayerInput>();
            attack = _playerInput.actions["Attack"];
            attack.performed += TryToAttack;
            block = _playerInput.actions["Block"];
            block.performed += TryToParry;
            
            _animator = GetComponent<Animator>();
        }

        public virtual void TryToAttack(InputAction.CallbackContext context)
        {
            Debug.Log("Attacked");

            if(currentState != State.Idle && currentState != State.Airborne && currentState != State.Attacking && !canInterruptState) //Check what state the player is in. Generally they'd need to be in one of the aforementioned states.
            {   return;}

            if (currentState == State.Attacking)
            {
                AirborneAttack();
            }

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
                    _animator.Play(ANIM_ATTACK_ONE); // Update these once we have the other anims. 
                    return;
                }
                case 2:
                {
                    _animator.Play(ANIM_ATTACK_ONE); 
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
                Vector2 otherSide = new Vector2(data.CombatData.weaponData.weaponOffset[0].x * -1,
                    data.CombatData.weaponData.weaponOffset[0].y);
                overlapping = Physics2D.OverlapBoxAll(transform.position + (Vector3)otherSide,
                    (Vector3)data.CombatData.weaponData.weaponRange[0], 
                    layersToCheck);
                //Debug.Log("Changed the side"+ otherSide);
            }
            else if(transform.rotation.y == 0)
            {
                overlapping = Physics2D.OverlapBoxAll(transform.position + (Vector3)data.CombatData.weaponData.weaponOffset[0],
                    (Vector3)data.CombatData.weaponData.weaponRange[0], 
                    layersToCheck);
            }
            
        
            foreach (var hit in overlapping)
            {
                if(hit.transform.root == transform) continue;
                
                if (hit.GetComponent<IDamageable>() != null)
                {
                    hit.GetComponent<IDamageable>().DealDamage(data.CombatData.attackPower, true);
                }
            }

            overlapping = null;

        }

        private void AirborneAttack()
        {
            _animator.Play("Airborne_Attack");
            
            //DO some checks to see if we hit anything
        }
        
        public void EndAttackSequence()
        {
            currentState = State.Idle;
            _animator.Play(ANIM_IDLE);
        }

        private void TryToParry(InputAction.CallbackContext context)
        {
            /*if (currentState != State.Idle) return;

            currentState = State.Blocking;
            
            _animator.Play(ANIM_BLOCK);*/
        }

        public void SpawnParryFrame()
        {
            overlapping =  Physics2D.OverlapBoxAll(transform.position + (Vector3)data.CombatData.parryOffset, (Vector3)data.CombatData.parryRange, 0,layersToCheck);

            if (overlapping == null) return;

            foreach (var enemy in overlapping)
            {
                var reference = enemy.GetComponent<EnemyBase>();
                reference.WasParried();
            }
            
        }

        //Used to visualize the range and position of attacks. Each attack can be configured from their data scriptable object. 
        public void OnDrawGizmosSelected()
        {
            if(data == null)
                return;

            if (data.CombatData.weaponData.ShowAttackDebug)
            {
                switch (data.CombatData.weaponData.ColliderType)
                {
                    case ColliderType.box:
                    {
                        for(int i = 0; i < data.CombatData.weaponData.weaponOffset.Length; i++)
                        {
                            Gizmos.DrawWireCube(transform.position + (Vector3)data.CombatData.weaponData.weaponOffset[i], (Vector3)data.CombatData.weaponData.weaponRange[i]);
                        }
                        break;
                    }
                    case ColliderType.circle:
                    {
                        for(int i = 0; i < data.CombatData.weaponData.weaponOffset.Length; i++)
                        {
                            Debug.Log("test");
                            Gizmos.DrawWireSphere(transform.position + (Vector3)data.CombatData.weaponData.weaponOffset[i], data.CombatData.weaponData.weaponRange[i].x);
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
               Gizmos.DrawWireCube(transform.position + (Vector3)data.CombatData.parryOffset, (Vector3)data.CombatData.parryRange);
            }

           
        }
    }
}