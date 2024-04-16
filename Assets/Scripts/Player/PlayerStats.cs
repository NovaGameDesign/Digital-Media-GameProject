using System;
using System.Collections;
using System.Linq;
using DigitalMedia.Combat;
using DigitalMedia.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DigitalMedia
{
    public class PlayerStats : StatsComponent
    {
        [Header("Dying UI")]
        [SerializeField] private GameObject dyingUI;
        [SerializeField] private GameObject deadUI;
        private PlayerCombatSystem _combatSystem;

        private PlayerInput _playerInput;
        private InputAction reload;
        private InputAction die;

        [Header("Healthbars")]
        [SerializeField] private Image healthBorder;
        [SerializeField] private Image healthFill;

        [SerializeField] private Sprite [] healthBorders;
        [SerializeField] private Sprite [] healthFills;

    private void Start()
    {
        _animator = GetComponent<Animator>();
            if (!overrideHealthMax)
            {
                health = data.BasicData.maxHealth;
            }
            
            vitality = data.BasicData.maxVitality;
            stamina = data.BasicData.stamina;
         
            healthbar = GetComponentInChildren<Slider>();

            _combatSystem = GetComponent<PlayerCombatSystem>();

            rb = this.gameObject.GetComponent<Rigidbody2D>();

            _playerInput = GetComponent<PlayerInput>();
            reload = _playerInput.actions["Revive"];
            die = _playerInput.actions["Die"];
            reload.started += PlayerWantsToLive;
            die.started += PlayerWantsToDie;
        }

        public void SwapHealthbarUI(int element)
        {
            healthBorder.sprite = healthBorders[element];
            healthFill.sprite = healthFills[element];
        }
        
        // Update is called once per frame
        public override void DealDamage(float incomingDamage, GameObject attackOrigin, Elements damageType, float knockbackForce = .5f, bool interruptAction = true)
        {
            //write a more complex damage function to account for defense, damage type, etc. 
            /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
            if (_combatSystem.blocking && damageType is not Elements.Holy)
            {
                incomingDamage -= (incomingDamage * data.CombatData.weaponData.innateWeaponBlock);
            }
            else if (damageType is Elements.Fire)
            {
                //Start coroutine to deal DOT 
            }
            else if (damageType is Elements.Ice)
            {
                DealVitalityDamage(incomingDamage/2);
            }
            
            _animator.Play("Player_Damaged");
            
            health -= incomingDamage;
            healthbar.value = health / data.BasicData.maxHealth;
            

            vitalityRegenerationSpeed = health / data.BasicData.maxHealth;
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
                //Debug.Log($"We tried to knock the player towards the left, the enemy was on the right. The desired knockback amount was {direction}");
            }
            else if (rb != null)
            {
                Vector2 direction = new Vector2(knockbackForce, rb.velocity.y); 
                rb.velocity = direction;
                StartCoroutine(BasicKBDelay(.1f));
                //Debug.Log($"We tried to knock the player towards the right, the enemy was on the left. The desired knockback amount was {direction}");
            }
            else
            {
                Debug.Log("RB was null");
            }
            
            if (health <= 0)
            {
                HandleLives();
            }
        }

        public override void HandleLives()
        {
            //1. Correct the values to match the new lives remaining and change health to max.
            currentLives--;
            InitiateStateChange(State.Dying);
            
            //2. Check if we now have too few lives to revive, if so start the death process. 
            if (currentLives <= 0)
            { 
                _animator.Play("Player_Death");
                
                Debug.Log("The player has died.");
                
                dyingUI.SetActive(false);
                deadUI.SetActive(true);
            }
            else
            {
                //Show the Dying UI to allow players the option to choose whether they want to live or die with left vs right click.
                dyingUI.SetActive(true);
                //change the input type to dying so we no longer accept any input but the two we consider "valid". Additionally we show the UI and reset the respawn value for later usage. 
                _playerInput.SwitchCurrentActionMap("Dying");
                //StartCoroutine(SelectDeathOption()); 

                //Set the time scale to 0 so no effects continue and attacks pause. 
                Time.timeScale = 0;
            }
        }

        public void PlayerWantsToDie(InputAction.CallbackContext context)
        {
            Time.timeScale = 1;
            _animator.Play("Player_Death");
            Debug.Log("The player has lost a life and chose to die.");
            dyingUI.SetActive(false);
            deadUI.SetActive(true);
        }
        
        public void PlayerWantsToLive(InputAction.CallbackContext context)
        {
            Time.timeScale = 1;
            dyingUI.SetActive(false);

            Debug.Log("The player has lost a life and chose to revive.");

            health = data.BasicData.maxHealth;
            vitality = data.BasicData.maxVitality;
            vitalityBar.value = 1;
            healthbar.value = 1;
            vitalityBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color(0, 1, 0.5437737f, 1);

            //2. Update UI to match lives remaining.
            livesUI.ElementAt(currentLives).SetActive(false);

            //3. Re-enabling enemy states if they were attacking -- or just turn their BTs back on.
            InitiateStateChange(State.Idle);
            
            _playerInput.SwitchCurrentActionMap("Player");
        }
        
        public void PlayerRespawn()
        {
            //Handle respawning 
            //Player needs to return to the checkpoint and start with full health and stats. 
            //For now we can just load the main scene again. 
            _playerInput.SwitchCurrentActionMap("Player");
            SceneManager.LoadScene("Main");
        }
    }
}
