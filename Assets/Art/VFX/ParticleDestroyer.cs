using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class ParticleDestroyer : MonoBehaviour
    {
        /*private ParticleSystem particle;
        private void Start()
        {
            particle = GetComponent<ParticleSystem>();
            float totalDuration = particle.duration + particle.startLifetime;
        }*/

        public void OnParticleSystemStopped()
        {
            this.gameObject.SetActive(false);
        }
    }
}
