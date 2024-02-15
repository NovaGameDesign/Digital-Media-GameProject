using UnityEngine;


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
    public class CoreCharacter : MonoBehaviour
    {
        public CharacterStats data;

        protected State currentState = State.Idle;
        protected bool canInterruptState;
        
        protected Animator _animator;

        //Any data that is shared across (almost) all versions of the player, NPCs, or enemies. 

        [SerializeField] protected LayerMask groundLayer;

        private void Start()
        {
            _animator = GetComponent<Animator>();
           
        }

        /// <summary>
        /// Pretty much a global way change the state. We shouldn't ever need to override this method, but in the event we do need to... it is virtual. 
        /// </summary>
        /// <param name="yesNo"></param>
        public virtual void CanInterruptState(int yesNo)
        {
            canInterruptState = yesNo == 1 ? true : false;  //Reads as: if yesno is 1 then true, else is false. 
        }
        
    }
}