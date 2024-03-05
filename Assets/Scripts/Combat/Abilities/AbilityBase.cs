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

        public virtual void OnAbilityUpdate(AbilityHolder holder) { }
        public abstract void Activate(AbilityHolder holder);

    }
}