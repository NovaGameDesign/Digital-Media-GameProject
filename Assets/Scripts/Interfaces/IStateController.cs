using System.Collections;
using System.Collections.Generic;
using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia
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
