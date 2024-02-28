using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class DeathblowSlash : MonoBehaviour
    {
        public float slashSpeed;
        private Rigidbody2D rb;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = new Vector2((transform.position.x + 1) * slashSpeed, transform.position.y);
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}
