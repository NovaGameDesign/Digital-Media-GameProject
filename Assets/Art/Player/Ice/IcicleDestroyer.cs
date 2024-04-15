using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class IcicleDestroyer : MonoBehaviour
    {
        private float currentAlpha = 1;
        private void OnTriggerEnter2D(Collider2D other)
        {
            StartCoroutine(DestroyObjectOverTime());
        }

        IEnumerator DestroyObjectOverTime()
        {
            yield return new WaitForSeconds(.05f);
            currentAlpha -= 0.1f;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,currentAlpha);
            if (currentAlpha <= 0)
            {
                Destroy(this.gameObject);

            }
            else
            {
                StartCoroutine(DestroyObjectOverTime());
            }
        }
    }
}
