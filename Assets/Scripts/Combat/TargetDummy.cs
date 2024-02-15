using System.Collections;
using DigitalMedia.Interfaces;
using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.Combat
{
    public class TargetDummy : CoreCharacter
    {
        protected Collider2D[] overlapping;

        [SerializeField] protected LayerMask layersToCheck;

        private int currentAttackIndex = 0;
        
        //Unlike the Player character's animation names, these are intended to be used across the base version of all that inherits from these. In other words, these should work on both the player and enemy. 
        private const string ANIM_IDLE = "Idle";
        private const string ANIM_ATTACK_ONE = "Attack_One";
        private const string ANIM_ATTACK_TWO = "Attack_Two";
        private const string ANIM_ATTACK_THREE = "Attack_Three";
        private const string ANIM_HIT_REACTION = "Hit_Reaction";
        private const string ANIM_BLOCK = "Block";

        private void Start()
        {
            _animator = GetComponent<Animator>();
            Invoke("TryToAttack",5); 
           
           
        }

        public void TryToAttack()
        {
            /*Debug.Log("Attacked");*/

            if(currentState != State.Idle && currentState != State.Airborne && currentState != State.Attacking && !canInterruptState) //Check what state the player is in. Generally they'd need to be in one of the aforementioned states.
            {   return;}

            switch (currentAttackIndex)
            {
                case 0:
                {
                    /*Debug.Log("Launched attack one");*/
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
            overlapping = Physics2D.OverlapCircleAll(transform.position, data.CombatData.weaponData.weaponRange[0].x,layersToCheck);
            

            foreach (var hit in overlapping)
            {
                //A Series of checks to see if we can or should damage the player. 
                if(hit.transform == transform) continue;
                
                if (hit.GetComponent<IDamageable>() == null) return;

                else if (hit.GetComponent<CombatSystem>().parrying && hit.GetComponent<ICombatCommunication>() != null)
                {
                    hit.GetComponent<ICombatCommunication>().DidParry();
                    WasParried();
                }
                
                else
                {
                    Debug.Log("Tried to attack a target, they were not parrying.");
                    hit.GetComponent<IDamageable>().DealDamage(data.CombatData.attackPower, this.gameObject, true);
                }
               
            }

            overlapping = null;
        }

        public void EndAttackSequence()
        {
            currentState = State.Idle;
            _animator.Play(ANIM_IDLE);

            Invoke("TryToAttack",5); 
        }

        public void WasParried()
        {
            Debug.Log("Tried to attack a target, they were parrying.");
            
            _animator.Play("Dummy_Hit");
            
            Invoke("TryToAttack",5); 
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
