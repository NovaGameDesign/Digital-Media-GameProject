using System.Collections;
using System.Collections.Generic;
using DigitalMedia.Combat;
using DigitalMedia.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMedia
{
    public class PlayerStats : StatsComponent
    {

        [SerializeField] private GameObject deathUI;
        private CombatSystem _combatSystem;
        
        private void Start()
        {
            if (!overrideHealthMax)
            {
                health = data.BasicData.maxHealth;
               
            }
            
            vitality = data.BasicData.maxVitality;
            stamina = data.BasicData.stamina;
         
            healthbar = GetComponentInChildren<Slider>();

            _combatSystem = GetComponent<CombatSystem>();

        }
        //add a health increase method here something like set healthbar to 1 for example for healing items
        // Update is called once per frame
        public override void DealDamage(float incomingDamage, GameObject attackOrigin, bool interruptAction = true)
        {
            
            //write a more complex damage function to account for defense, damage type, etc. 
            /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
            if (_combatSystem.blocking)
            {
                incomingDamage -= (incomingDamage * data.CombatData.weaponData.innateWeaponBlock);
            }
            
            health -= incomingDamage;
            healthbar.value = health / data.BasicData.maxHealth;
            

            if (health <= 0)
            { 
                Time.timeScale = 0f;
                deathUI.SetActive(true);
            }
        }
    }
}
