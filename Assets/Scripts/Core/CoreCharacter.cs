using DigitalMedia.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMedia.Core
{
    public enum State
    {
        None,
        Idle,
        Airborne,
        Attacking,
        Blocking,
        Staggered,
        
    }
    public class CoreCharacter : MonoBehaviour, IDamageable
    {
        public CharacterStats data;

        protected State currentState = State.Idle;
        protected bool canInterruptState;
        
        protected Animator _animator;

        [SerializeField] protected float _health;

        protected Slider healthbar;

        //Any data that is shared across (almost) all versions of the player, NPCs, or enemies. 

        [SerializeField] protected LayerMask groundLayer;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _health = data.BasicData.maxHealth;
            healthbar = GetComponentInChildren<Slider>();
        }

        /// <summary>
        /// Pretty much a global way change the state. We shouldn't ever need to override this method, but in the event we do need to... it is virtual. 
        /// </summary>
        /// <param name="yesNo"></param>
        public virtual void CanInterruptState(int yesNo)
        {
            canInterruptState = yesNo == 1 ? true : false;  //Reads as: if yesno is 1 then true, else is false. 
        }
        
        public virtual void DealDamage(float incomingDamage, bool interruptAction = true)
        {
            //write a more complex damage function to account for defense, damage type, etc. 
            /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
            _health -= incomingDamage;
            healthbar.value = _health / data.BasicData.maxHealth;

            if (_health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}