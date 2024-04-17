using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class Trapdoor : MonoBehaviour
    {
        [SerializeField] private GameObject tilesToHide;

        private void OnTriggerEnter2D(Collider2D other)
        {
            tilesToHide.SetActive(false);
        }
    }
}
