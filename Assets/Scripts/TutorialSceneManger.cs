using System;
using System.Collections;
using System.Collections.Generic;
using DigitalMedia.Combat;
using DigitalMedia.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DigitalMedia
{
    public class TutorialSceneManger : MonoBehaviour
    {
        [SerializeField] private GameObject[] tutorialUI;
        private int tutorialIndex;

        [SerializeField] private PlayerController _playerController;

        [SerializeField] private PlayerCombatSystem _playerCombatSystem;
        
        private PlayerInput _playerInput;
        public InputAction move;
        private InputAction jump;
        private InputAction dodge;
        private InputAction dash;
        private InputAction attack;
        private InputAction block;
        private InputAction swapElement;
        
        
        // Start is called before the first frame update
        void Start()
        {
            
            _playerInput = _playerController.gameObject.GetComponent<PlayerInput>();
            move = _playerInput.actions["Move"];
            jump = _playerInput.actions["Jump"];
            dash = _playerInput.actions["Dash"];
            attack = _playerInput.actions["Attack"];
            block = _playerInput.actions["Block"];
            swapElement = _playerInput.actions["Swap Element"];
            
            //Assigning Functionality

            jump.performed += PlayerJumped;
            
            dash.performed += PlayerDashed;
            
            move.performed += PlayerMoved;
            
            attack.performed += PlayerAttacked;
            
            block.performed += PlayerBlocked;
            
            swapElement.performed += PlayerSwappedElements;

            
        }

        private void OnDisable()
        {
            jump.performed -= PlayerJumped;
            jump.Disable(); 
            
            dash.performed -= PlayerDashed;
            dash.Disable();
            
            move.performed -= PlayerMoved;
            move.Disable();
            
            attack.performed -= PlayerAttacked;
            attack.Disable();
            
            block.performed -= PlayerBlocked;
            block.Disable();
            
            swapElement.performed -= PlayerSwappedElements;
            swapElement.Disable();
        }
        

        // Update is called once per frame
        void Update()
        {
            if (_playerController.canWallJump && tutorialIndex == 3)
            {
                
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }

            if (_playerController.currentState == State.Deathblowing && tutorialIndex == 5)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());  
            }

            if (_playerController.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Player_Deathblow") && tutorialIndex == 8)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
        }

        private void PlayerMoved(InputAction.CallbackContext context)
        {
            if (tutorialIndex == 0)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
        }

        private void PlayerJumped(InputAction.CallbackContext context)
        {
            if (tutorialIndex == 2)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
        }
        
        private void PlayerDashed(InputAction.CallbackContext context)
        {
            if (tutorialIndex == 1)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
        }
        
        private void PlayerAttacked(InputAction.CallbackContext context)
        {
            if (tutorialIndex == 4)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
            else if (tutorialIndex == 6 && !_playerController.IsGrounded())
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
        }
        
        private void PlayerBlocked(InputAction.CallbackContext context)
        {
            if (tutorialIndex == 7)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
           
        }
        private void PlayerSwappedElements(InputAction.CallbackContext context)
        {
            if (tutorialIndex == 9)
            {
                tutorialIndex++;
                StartCoroutine(shortDelay());
            }
        }
        

        IEnumerator shortDelay()
        {
            yield return new WaitForSeconds(1f);
            foreach (var UI in tutorialUI)
            {
                UI.SetActive(false);
            }
            
            tutorialUI[tutorialIndex].SetActive(true);
        }
    
    }
}
