using System.Collections;
using DigitalMedia.Core;
using UnityEngine;
using UnityEngine.Events;

namespace DigitalMedia.Combat.Abilities
{
    public class AbilityHolder : MonoBehaviour
    {
        public CoreCharacter owner;

        public AbilityBase ability;

        public AbilityStates currentAbilityState = AbilityStates.ReadyToActivate;

        public UnityEvent onTriggerAbility;

        private Coroutine _handleAbilityUsage; //The referenced I used showed this as an IEnumerator but this threw an error, swapping it to a coroutine seemed to fix it. 
        
        public enum AbilityStates
        {
            ReadyToActivate = 0,
            Using = 1, 
            OnCooldown = 2
        }

        public void TriggerAbility()
        {
            if (currentAbilityState != AbilityStates.ReadyToActivate)
                return;
            if (!CharacterIsInAllowedState())
                return;

            _handleAbilityUsage = StartCoroutine(HandleAbilityUsage_CO());
        }

        private IEnumerator HandleAbilityUsage_CO()
        {
            currentAbilityState = AbilityStates.Using;

            yield return new WaitForSeconds(ability.castTime);
            
            //Functionality stored in the Scriptable Object that performs the ability's logic. 
            ability.Activate(this);

            //Swap the ability's state to cooldown
            currentAbilityState = AbilityStates.OnCooldown;
            
            //If we have any Unity Methods, use them. 
            onTriggerAbility?.Invoke();

            if (ability.hasCooldown)
            {
                StartCoroutine(HandleCooldown_CO());
            }
        }
        
        private IEnumerator HandleCooldown_CO()
        {
            yield return new WaitForSeconds(ability.cooldown);

            currentAbilityState = AbilityStates.ReadyToActivate;
        }
        public bool CharacterIsInAllowedState()
        {
            return ability.allowedUsageStates.Contains(owner.currentState);
        }

    }
}