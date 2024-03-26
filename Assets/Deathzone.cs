using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class Deathzone : MonoBehaviour
    {
        public Transform respawnPoint;
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.transform.position = respawnPoint.position;
        }
    }
}
