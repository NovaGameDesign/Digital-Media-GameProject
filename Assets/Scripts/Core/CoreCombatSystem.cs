using System;
using System.Collections;
using AYellowpaper.SerializedCollections;
using DigitalMedia.Combat;
using DigitalMedia.Combat.Abilities;
using DigitalMedia.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DigitalMedia.Core
{
    public class CoreCombatSystem : CoreCharacter
    {
        protected Collider2D[] overlapping;

        [SerializeField] protected LayerMask layersToCheck;

        [NonSerialized] public int currentAttackIndex = 0;
        public bool parrying;
        [System.NonSerialized] public bool blocking;
        private GameObject targetThatParried;
        
        //Unlike the Player character's animation names, these are intended to be used across the base version of all that inherits from these. In other words, these should work on both the player and enemy. 

        #region Attack Related Functionality

        [NonSerialized] public StatsComponent stats;

        #region Ability System Variables and enums 
        
        [SerializedDictionary("Attack Name", "Attack Scriptable Object Ref")]
        public SerializedDictionary<string, AbilityBase> abilities;

        protected AbilityBase currentAbility;
        protected AbilityBase oldAbility; 
        
        #endregion
        
      
        void Start()
        {
            _animator = GetComponent<Animator>();
            _audioPlayer = GetComponent<AudioSource>();

            stats = GetComponent<StatsComponent>();

            InitiateStateChange(State.Idle);
        }
        
        public virtual void TryToAttack(string desiredAttack)
        {
            TriggerAbility(desiredAttack); 
        }
        
        public void TriggerAbility(string desiredAbility)
        {
            //Store the old ability in case we get in trouble. 
            oldAbility = currentAbility;
            
            //Find the desired ability 
            abilities.TryGetValue(desiredAbility, out currentAbility);
            if (currentAbility == null) return;
            
            
            //Debug.Log($"The ability we found was {currentAbility}. The owning script is {this.gameObject}");

            if (currentAbility.currentAbilityState != AbilityBase.AbilityStates.ReadyToActivate)
            {
                currentAbility = oldAbility;
                return;  
            }
            if (!CharacterIsInAllowedState() && !canInterruptState)
            {
                //If we can't use the new ability then we need to swap back to the old ability in case it had any additional checks to complete.
                //Debug.Log($"The player was not in the right state, they were in {currentState}");
                currentAbility = oldAbility;
                return;
            }
            
            //play animation 
            _animator.Play(currentAbility.animToPlay.name);
            InitiateStateChange(State.Attacking);
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
            currentAttackIndex = 0;
            InitiateStateChange(State.Idle);
            _animator.Play("Idle");
            
            if (currentAbility.hasCooldown)
            {
                currentAbility.currentAbilityState = AbilityBase.AbilityStates.OnCooldown;
                StartCoroutine(HandleCooldown_CO());
            }
        }
        
        public IEnumerator HandleCooldown_CO()
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
                overlapping = Physics2D.OverlapBoxAll(transform.position + (Vector3)otherSide, weaponRange, layersToCheck);
                //Debug.Log("Changed the side"+ otherSide);
            }
            else if (transform.rotation.y == 0)
            {
                overlapping = Physics2D.OverlapBoxAll(transform.position + (Vector3)weaponOffset, weaponRange, layersToCheck);
            }


            foreach (var hit in overlapping)
            {
                if (hit.transform.root == transform || hit.transform == transform) continue; //Checks if it hit self. 
                
                // If the target is deathblowing we don't want to deal any damage to them. 
                if (hit.GetComponent<CoreCombatSystem>()?.currentState == State.Deathblowing) //Checks if the target is deathblow -- in other words whether we should deal damage.  
                {
                    overlapping = null;
                    return; 
                }

                if (hit.GetComponent<CoreCombatSystem>() != null && hit.GetComponent<CoreCombatSystem>().parrying) // Checks if the target is parrying. 
                {
                    //In this call we need to be able to specify what KIND of attack was parried, range, melee, strong, etc. 
                    hit.GetComponent<ICombatCommunication>().DidParry(); //Calling on the target
                    //Add functionality to change the parry reaction based on the given attack type. Also add a paramater for if the attack should even be interrupted or not. 
                    WasParried();
                    //Check if the ability has any additional functionality it needs to execute after being parried. 
                    if (stats.vitality < 0)
                    {
                        InitiateStateChange(State.Staggered);
                        _animator.Play("Staggered");
                        if (this.gameObject.name == "Player") return;
                        
                        hit.GetComponent<PlayerCombatSystem>().deathblowTarget = this.gameObject;
                        targetThatParried = hit.gameObject;

                        Time.timeScale = 0.25f;
                    }
                    overlapping = null;
                    return;
                }
                
                if (hit.GetComponent<IDamageable>() != null)
                {
                    var damage = data.CombatData.weaponData.innateWeaponDamage * data.CombatData.attackPower;
                    hit.GetComponent<IDamageable>().DealDamage(damage, this.gameObject, currentAbility.knockBackAmount, true);
                    int soundToPlay = Random.Range(0, hit.GetComponent<CoreCharacter>().data.CombatData.damageSounds.Length);
                    _audioPlayer.PlayOneShot(hit.GetComponent<CoreCharacter>().data.CombatData.damageSounds[soundToPlay]);
                    
                }
            }

            overlapping = null;
        }
        
        /// <summary>
        /// Deprecated code. Some animations may still reference it. 
        /// </summary>
        public void EndAttackSequence()
        {
            InitiateStateChange(State.Idle);
            currentAttackIndex = 0;
            _animator.Play("Idle");
        }

        /// <summary>
        /// Called via the animation associated with the deathblow. 
        /// </summary>
        public void DisableDeathblow()
        {
            InitiateStateChange(State.Idle);
            _animator.Play("Idle");
            targetThatParried.GetComponent<PlayerCombatSystem>().deathblowTarget = null;
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

        public void ChangeTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
    }
}