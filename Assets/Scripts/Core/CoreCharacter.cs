using DigitalMedia.Interfaces;
using UnityEngine;

namespace DigitalMedia.Core
{
    public enum State
    {
        None,
        Idle,
        Moving,
        Airborne,
        Attacking,
        Deathblowing,
        Blocking,
        Staggered,
    }

    public class CoreCharacter : MonoBehaviour, IStateController
    {
        public CharacterData data;
        
        protected bool canInterruptState;
        
        protected Animator _animator;
        protected AudioSource _audioPlayer;

        //Any data that is shared across (almost) all versions of the player, NPCs, or enemies. 

        [SerializeField] protected LayerMask groundLayer;

        protected string currentAnimState;
        [System.NonSerialized] public State currentState = State.Idle;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioPlayer = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Pretty much a global way change the state. We shouldn't ever need to override this method, but in the event we do need to... it is virtual. 
        /// </summary>
        /// <param name="yesNo"></param>
        public virtual void CanInterruptState(int yesNo)
        {
            canInterruptState = yesNo == 1 ? true : false;  //Reads as: if yesno is 1 then true, else is false. 
        }
        
        public virtual void ChangeAnimationState(string newState)
        {
            //Checks if the animation is already playing 
            if (newState == currentAnimState) return;

            //Plays a new animation
            _animator.Play(newState);

            //Sets the current animation for later use. 
            currentAnimState = newState;
        }

        public void StateChanger(State changeToState)
        {
            currentState = changeToState;
        }

        public void InitateStateChange(State changeStateTo)
        {
            var childrenWithInterface = gameObject.GetComponents<IStateController>();
            foreach (var item in childrenWithInterface)
            {
                item.StateChanger(changeStateTo);
            }
        }
    }
}