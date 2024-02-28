using System;
using System.Collections;
using DigitalMedia.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMedia.Core
{
    public class StatsComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] protected CharacterData data;

        [SerializeField] private bool overrideHealthMax;
        public float health;
        //Attacking
        protected float stamina;
        //Posture
        public float vitality;
        
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

        [SerializeField] private GameObject blood;
 
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

        public virtual void DealDamage(float incomingDamage, GameObject attackOrigin, bool interruptAction = true)
        {
            //write a more complex damage function to account for defense, damage type, etc. 
            /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
            health -= incomingDamage;
            healthbar.value = health / data.BasicData.maxHealth;

            Instantiate(blood, gameObject.transform);
            if (attackOrigin.transform.position.x > gameObject.transform.position.x && gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Vector2 direction = new Vector2(-.5f, transform.position.y);
                gameObject.GetComponent<Rigidbody2D>().velocity = direction;
                StartCoroutine(BasicKBDelay(.1f));
       
            }
            else if (gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Vector2 direction = new Vector2(.5f, transform.position.y);
                gameObject.GetComponent<Rigidbody2D>().velocity = direction;
                StartCoroutine(BasicKBDelay(.1f));
                
            }
            
            if (health <= 0)
            {
                if (this.gameObject.name == "Player") return;
                //spawn other things like guts and blood!
                Destroy(this.gameObject);
            }
        }

        public void DealVitalityDamage(float incomingVitalityDamage)
        {
            vitality -= incomingVitalityDamage;

            vitalityBar.value = vitality / data.BasicData.maxVitality;
            
            if (vitality <= 0)
            {
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
        
        IEnumerator BasicKBDelay(float delayTime)
        {
            //Wait for the specified delay time before continuing.
            
            yield return new WaitForSeconds(delayTime);
            
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Debug.Log("We stopped the velocity");
            //Do the action after the delay time has finished.
        }
    }
}
