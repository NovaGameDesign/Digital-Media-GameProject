using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigitalMedia.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMedia.Core
{
    public class StatsComponent : CoreCharacter, IDamageable
    {
        [SerializeField] protected bool overrideHealthMax;
        public float health;
        public int currentLives;
        [SerializeField] protected int maxLives;
        
        //Attacking
        protected float stamina;
        //Posture
        public float vitality;
        public float vitalityRegenerationSpeed;
        
        protected Slider healthbar;
        public List<GameObject> livesUI;

        [SerializeField]
        protected Slider vitalityBar;

        [SerializeField]
        private GameObject statsUI;

        private float damageOverTimeTimer;
        
        //[System.NonSerialized]
        private bool _inCombat;
        public bool inCombat
        {
            get { return _inCombat; }
            set
            {
                if (_inCombat != value)
                {
                    //Debug.Log("A new value was set for inCombat, the new value is "+value);
                    _inCombat = value;
                    statsUI.SetActive(_inCombat);
                }
            }
        }

        [SerializeField] protected GameObject blood;

        protected Rigidbody2D rb;
 
        private void Start()
        {
            if (!overrideHealthMax)
            {
                health = data.BasicData.maxHealth;
            }
            
            vitality = data.BasicData.maxVitality;
            stamina = data.BasicData.stamina;
         
            healthbar = GetComponentInChildren<Slider>();

            rb = this.gameObject.GetComponent<Rigidbody2D>();
        }

        public virtual void DealDamage(float incomingDamage, GameObject attackOrigin, Elements damageType, float knockbackForce = .5f, bool interruptAction = true)
        {
            //write a more complex damage function to account for defense, damage type, etc. 
            /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
            if (currentState == State.Dying) return;
            
            if (damageType is Elements.Fire)
            {
                //Start coroutine to deal DOT 
                damageOverTimeTimer = Time.time;
                StartCoroutine(DamageOverTime(1, 3));
            }
            else if (damageType is Elements.Ice)
            {
                DealVitalityDamage(incomingDamage/4);
                knockbackForce *= 2;
            }
            
            health -= incomingDamage;
            healthbar.value = health / data.BasicData.maxHealth;

            vitalityRegenerationSpeed = health / data.BasicData.maxHealth / 1.25f;
            if (vitalityRegenerationSpeed <= 0.25f)
            {
                Color newColor = new Color(1, .5f, 0f, 1);
                vitalityBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = newColor;
            }
            else if (vitalityRegenerationSpeed <= 0.5)
            {
                vitalityBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                vitalityBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color(0, 1, 0.5437737f, 1);
            }
            

            Instantiate(blood, gameObject.transform);
            if (attackOrigin.transform.position.x > gameObject.transform.position.x && rb != null)
            {
                Vector2 direction = new Vector2(-knockbackForce, rb.velocity.y); 
                rb.velocity = direction;
                StartCoroutine(BasicKBDelay(.1f));
       
            }
            else if (rb != null)
            {
                Vector2 direction = new Vector2(knockbackForce, rb.velocity.y);
                rb.velocity = direction;
                StartCoroutine(BasicKBDelay(.1f));
            }
            
            if (health <= 0)
            {
                HandleLives();
            }
        }

        IEnumerator DamageOverTime(float damage, float duration)
        {
            
            yield return new WaitForSeconds(.1f);
            
            health -= damage;
            healthbar.value = health / data.BasicData.maxHealth;
            
            // Continue to check if the correct time has passed. 
            if (Time.time < damageOverTimeTimer + duration)
            {
                StartCoroutine(DamageOverTime(damage, duration));
            }
        }
        
        
        //I honestly had no clue what to call this. There may be a better name out there. 
        public virtual void HandleLives()
        {
            //1. Correct the values to match the new lives remaining and change health to max.
            currentLives -= 1;
            if (currentLives <= 0)
            {
                //if (this.gameObject.name == "Player") return;
                //spawn other things like guts and blood!
                Destroy(this.gameObject);
            }
            
            InitiateStateChange(State.Dying);
            
            health = data.BasicData.maxHealth;
            vitality = data.BasicData.maxVitality;
            vitalityBar.value = 1;
            healthbar.value = 1;
            vitalityBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color(0, 1, 0.5437737f, 1);

            //2. Update UI to match lives remaining.
            livesUI.ElementAt(currentLives).SetActive(false);
            
            //3. Re-enabling enemy states if they were attacking -- or just turn their BTs back on. 
            InitiateStateChange(State.Idle);
            
            //4. Check if there are any lives remaining and destroy the object if needed. Theoretically we may need to do it first.
           
        }

        public void DealVitalityDamage(float incomingVitalityDamage)
        {
            vitality -= incomingVitalityDamage;

            vitalityBar.value = vitality / data.BasicData.maxVitality;

            StopAllCoroutines();
            StartCoroutine(BasicVitalityDelay(0.25f));
        }

        private IEnumerator BasicVitalityDelay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            
            StartCoroutine(RegenerateVitality());
        }
        
        private IEnumerator RegenerateVitality()
        {
            yield return new WaitForSeconds(.1f);

            vitality += vitalityRegenerationSpeed;
            vitalityBar.value = vitality / data.BasicData.maxVitality;
            
            if (vitality < data.BasicData.maxVitality)
            {
                StartCoroutine(RegenerateVitality());
            }
            else
            {
                StopCoroutine(RegenerateVitality());
            }
        }
        
        public IEnumerator BasicKBDelay(float delayTime)
        {
            //Wait for the specified delay time before continuing.
            
            yield return new WaitForSeconds(delayTime);
            
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            //Debug.Log("We stopped the velocity");
            //Do the action after the delay time has finished.
        }
    }
}
