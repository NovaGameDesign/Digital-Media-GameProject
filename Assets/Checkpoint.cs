using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalMedia
{
    public class Checkpoint : MonoBehaviour
    {
        private bool checkpointActive;
        // Start is called before the first frame update
        private void OnEnable()
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += RepositionPlayer;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            checkpointActive = true;
        }

        private void RepositionPlayer(Scene scene, LoadSceneMode mode)
        {
            if (checkpointActive)
            {
                GameObject.Find("Player").transform.position = transform.position;
            }
        }
    }
}
