using System.Collections.Generic;
using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    public abstract class AbilityBase : ScriptableObject
    {
        [Header("Ability Details ")]
        public string abilityName;
        
        [Header("Cooldown and Cast Time")] 
        public bool hasCooldown = true;
        public float cooldown = 1f;
        public float castTime = 0f;

        [Header("Allowed Ability Usage States")]
        public List<State> allowedUsageStates = new List<State>() { State.Idle };

        [Header("Animation Details")]
        public AnimationClip animToPlay;

        [Header("Sounds")]
        public AudioClip sfxBasic;
        public AudioClip sfxHit;
        
        public float knockBackAmount;

        public AbilityStates currentAbilityState = AbilityStates.ReadyToActivate;
        
        public enum AbilityStates
        {
            ReadyToActivate = 0,
            Using = 1, 
            OnCooldown = 2
        }
        
        public virtual void OnAbilityUpdate(AbilityHolder holder) { }

        public abstract void Activate(GameObject holder);

        public virtual void ActivateAbilityEffects(GameObject holder)
        {
           holder.gameObject.GetComponent<AudioSource>()?.PlayOneShot(sfxBasic);
        }

    }
}