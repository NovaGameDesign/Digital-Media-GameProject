using UnityEngine;
using DigitalMedia.Core;

namespace DigitalMedia.Interfaces
{
    public interface IStateController
    {
        /// <summary>
        /// Used to Change the state of each script that inherits from CoreCharacter. 
        /// </summary>
        /// <param name="changeToState"></param>
        public void StateChanger(State changeToState);
    }
}
