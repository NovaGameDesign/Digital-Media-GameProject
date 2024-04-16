using System;
using System.Collections;
using System.Collections.Generic;
using DigitalMedia.Combat;
using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia
{
    public class TransitionController : MonoBehaviour
    {
        // Start is called before the first frame update
        private void OnEnable()
        {
            gameObject.GetComponent<Animator>().Play("Cocoon-Transition");
        }

        public void CocoonLoop()
        {
            transform.parent.GetComponent<Animator>().Play("Idle");
            transform.parent.GetComponent<EnemyCoreCombat>().InitiateStateChange(State.Idle);
            this.gameObject.SetActive(false);
        }

        public void SwapToWings()
        {
            transform.parent.GetComponent<EnemyCoreCombat>().ForceElementChange();
            
        }
    }
}
