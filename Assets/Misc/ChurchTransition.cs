using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class ChurchTransition : MonoBehaviour
    {
        private AudioSource audioSource;

        public GameObject ui;

        private bool played = false;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!played)
            {
                audioSource.Play();
                ui.SetActive(true);
                StartCoroutine(UIDelay());
                played = true;
            }
          
        }

        private IEnumerator UIDelay()
        {
            yield return new WaitForSeconds(7f);
            ui.SetActive(false);
        }
    }
}
