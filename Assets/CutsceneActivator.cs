using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace DigitalMedia
{
    public class CutsceneActivator : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == "Player")
            {
                transform.parent.GetComponent<PlayableDirector>().Play();
            }
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
