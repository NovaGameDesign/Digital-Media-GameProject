using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class BossDiamond : MonoBehaviour
    {
        public GameObject bossRef;

        void OnTriggerEnter2D(Collider2D col)
        {
            bossRef.GetComponent<Animator>().Play("Auriel_Summoning");
            bossRef.SetActive(true);
            //bossRef.GetComponent<Animator>().Play("Summoning");
            Destroy(this.gameObject);
        }
    }
}
