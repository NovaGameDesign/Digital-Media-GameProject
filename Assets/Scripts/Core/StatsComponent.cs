using System;
using DigitalMedia.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMedia.Core
{
    public class StatsComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private CharacterStats data;

        [SerializeField] private bool overrideHealthMax;
        public float health;
        protected float stamina;
        protected float vitality;
        
        protected Slider healthbar;

        private void Start()
        {
            if (!overrideHealthMax)
            {
                health = data.BasicData.maxHealth;
            }
         
            healthbar = GetComponentInChildren<Slider>();
        }

        public void DealDamage(float incomingDamage, GameObject attackOrigin, bool interruptAction = true)
        {
            //write a more complex damage function to account for defense, damage type, etc. 
            /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
            health -= incomingDamage;
            healthbar.value = health / data.BasicData.maxHealth;

            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
