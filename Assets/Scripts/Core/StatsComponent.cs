using System;
using System.Collections;
using DigitalMedia.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMedia.Core
{
    public class StatsComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private CharacterData data;

        [SerializeField] private bool overrideHealthMax;
        public float health;
        protected float stamina;
        protected float vitality;
        
        protected Slider healthbar;

        [SerializeField]
        protected Slider vitalityBar;

        [SerializeField]
        private GameObject statsUI;
        
        //[System.NonSerialized]
        private bool _inCombat;
        public bool inCombat
        {
            get { return _inCombat; }
            set
            {
                if (_inCombat != value)
                {
                    Debug.Log("A new value was set for inCombat, the new value is "+value);
                    _inCombat = value;
                    statsUI.SetActive(_inCombat);
                }
            }
        }

        private void Start()
        {
            if (!overrideHealthMax)
            {
                health = data.BasicData.maxHealth;
               
            }
            vitality = data.BasicData.maxVitality;
            stamina = data.BasicData.stamina;
         
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
                if (this.gameObject.name == "Player") return;
                Destroy(this.gameObject);
            }
        }

        public void DealVitalityDamage(float incomingVitalityDamage)
        {
            vitality -= incomingVitalityDamage;

            vitalityBar.value = vitality / data.BasicData.maxVitality;
            
            if (vitality <= 0)
            {
                /*if (this.gameObject.tag("Enemy"))
                {
                    
                }*/
                
                //do something 
            }

            //StartCoroutine(RegenerateVitality());
        }

        private IEnumerator RegenerateVitality()
        {
            yield return new WaitForSeconds(data.BasicData.vitalityRegenSpeed);

            vitality += 1;

            if (vitality <= data.BasicData.maxVitality)
            {
                StartCoroutine(RegenerateVitality());
            }
        }
    }
}
