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

        [SerializeField] private GameObject dyingUi;
        [SerializeField] private GameObject deadUI;
        private PlayerCombatSystem _combatSystem;


        private PlayerInput _playerInput;
        private InputAction reload;
        private InputAction die;
        private int respawn = -1;
        
        private void Start()
        {
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
            reload = _playerInput.actions["Reload"];
            die = _playerInput.actions["Die"];
            reload.performed += PlayerWantsTolive;
            die.performed += PlayerWantsToDie;
        }

        
        private void OnEnable()
        {
            reload.Enable();
            die.Enable();
        }

        private void OnDisable()
        {
            reload.Dispose();
            die.Dispose();
        }

        // Update is called once per frame
        public override void DealDamage(float incomingDamage, GameObject attackOrigin, float knockbackForce = .5f, bool interruptAction = true)
        {
            //write a more complex damage function to account for defense, damage type, etc. 
            /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
            if (_combatSystem.blocking)
            {
                incomingDamage -= (incomingDamage * data.CombatData.weaponData.innateWeaponBlock);
            }
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
            
            if (attackOrigin.transform.position.x > gameObject.transform.position.x && gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Vector2 direction = new Vector2(-knockbackForce, GetComponent<Rigidbody2D>().velocity.y); 
                GetComponent<Rigidbody2D>().velocity = direction;
                StartCoroutine(BasicKBDelay(.1f));
                Debug.Log($"We tried to knock the player towards the left, the enemy was on the right. The desired knockback amount was {direction}");
            }
            else if (gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Vector2 direction = new Vector2(knockbackForce, GetComponent<Rigidbody2D>().velocity.y); 
                GetComponent<Rigidbody2D>().velocity = direction;
                StartCoroutine(BasicKBDelay(.1f));
                Debug.Log($"We tried to knock the player towards the right, the enemy was on the left. The desired knockback amount was {direction}");
            }
            else
            {
                Debug.Log("RB was null");
            }
            
            if (health <= 0)
            {
                HandleLives();
                /*Time.timeScale = 0f;
                dyingUi.SetActive(true);*/
            }
        }

        public override void HandleLives()
        {
            //1. Correct the values to match the new lives remaining and change health to max.
            currentLives -= 1;
            if (currentLives <= 0)
            { 
                Debug.Log("The player has died.");
                deadUI.SetActive(true);
                deadUI.GetComponent<Animator>()?.Play("Default");
                PlayerRespawn();
            }

            Time.timeScale = 0;
            PlayerReboot();
        }

        private void PlayerReboot()
        {
            _playerInput.SwitchCurrentActionMap("Dying");
            respawn = -1;
            
            dyingUi.SetActive(true);

            StartCoroutine(WaitForDecision());
            
            dyingUi.SetActive(false);
            
            //Player wants to die
            if (respawn == 0)
            {
                Debug.Log("The player has lost a life and chose to die.");

                deadUI.SetActive(true);
                deadUI.GetComponent<Animator>()?.Play("Default");
                PlayerRespawn();
            }
            else if (respawn == 1) //Player Wants to live
            {
                Debug.Log("The player has lost a life and chose to revive.");

                health = data.BasicData.maxHealth;
                vitality = data.BasicData.maxVitality;
                vitalityBar.value = 1;
                healthbar.value = 1;
                vitalityBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color(0, 1, 0.5437737f, 1);
            
                //2. Update UI to match lives remaining.
                livesUI.ElementAt(currentLives).SetActive(false);
            
                //3. Re-enabling enemy states if they were attacking -- or just turn their BTs back on. 
                InitateStateChange(State.Idle);
                _playerInput.SwitchCurrentActionMap("Player");
            }
        }

        private void PlayerWantsToDie(InputAction.CallbackContext context)
        {
            respawn = 0;
        }
        private void PlayerWantsTolive(InputAction.CallbackContext context)
        {
            respawn = 1;
        }
        private IEnumerator WaitForDecision()
        {
            respawn = -1;
            while (respawn == -1)
            {
                
            }
            
            yield return null;
            
        }
        public void PlayerRespawn()
        {
            //Handle respawning 
            //Player needs to return to the checkpoint and start with full health and stats. 
            //For now we can just load the main scene again. 
            SceneManager.LoadScene("Main");
        }
    }
}
