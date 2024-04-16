using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class PlayerDeathExplosion : MonoBehaviour
    {
        public float explosionDistance;
        private Rigidbody2D rb;
        private float xDir;
        private float yDir;
        private float torque;
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            xDir = Random.Range(-explosionDistance, explosionDistance);
            yDir = Random.Range(-explosionDistance, explosionDistance);
            torque = Random.Range(5, 15);

            rb.AddForce(new Vector2(xDir, yDir), ForceMode2D.Impulse);
            rb.AddTorque(torque, ForceMode2D.Force);
        }
    }
}
