using System.Collections;
using DigitalMedia.Combat.Abilities;
using DigitalMedia.Core;
using DigitalMedia.Interfaces;
using UnityEngine;

namespace DigitalMedia
{
    public class EEBeam : MonoBehaviour
    {
        public EE_AttackOne abilityRef;
        
        private Transform startingPosition;

        private Rigidbody2D rb;

        public float speedX;

        private AudioSource _audioPlayer;
        private void Start()
        {
           
            rb = GetComponent<Rigidbody2D>();
            _audioPlayer = GetComponent<AudioSource>();
            startingPosition = transform.parent.transform;
        }

        private IEnumerator veryShortDelay()
        {
            yield return new WaitForSeconds(.5f);
            startingPosition = transform;
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(transform.rotation.y <= 0 ? speedX : -speedX, 0);
       
            if (Vector2.Distance(transform.position, startingPosition.position) > 30)
            {
                Destroy(this.gameObject);
            }
            
            //Debug.Log($"The current distance to the object is: {distance} with a starting position of: {startingPosition.position}"); 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log(other.name);
            var hit = other;
            
            if (hit.transform.root == transform || hit.transform == transform) return;
            
            // If the target is deathblowing we don't want to deal any damage to them. 
            if (hit.GetComponent<CoreCombatSystem>()?.currentState == State.Deathblowing || hit.GetComponent<CoreCombatSystem>() == null) //Checks if the target is deathblow -- in other words whether we should deal damage.  
            {
                Destroy(this.gameObject);
                return; 
            }

            if (hit.GetComponent<CoreCombatSystem>() != null && hit.GetComponent<CoreCombatSystem>().parrying) // Checks if the target is parrying. 
            {
                //In this call we need to be able to specify what KIND of attack was parried, range, melee, strong, etc. 
                hit.GetComponent<ICombatCommunication>().DidParry(); //Calling on the target
                //Add functionality to change the parry reaction based on the given attack type. Also add a paramater for if the attack should even be interrupted or not. 
                
                //Check if the ability has any additional functionality it needs to execute after being parried. 
                Destroy(this.gameObject);
                return;
            }
            
            if (hit.GetComponent<IDamageable>() != null)
            {
                hit.GetComponent<IDamageable>().DealDamage(abilityRef.beamDamage, this.gameObject, Elements.Default, abilityRef.knockBackAmount, true);
                _audioPlayer.PlayOneShot(abilityRef.sfxHit);
                Destroy(this.gameObject);
            }
        }
    }
}
