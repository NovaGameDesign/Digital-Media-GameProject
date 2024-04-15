using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class IcicleSpawner : MonoBehaviour
    {
        [SerializeField] private Sprite[] possibleSprites;
        [SerializeField] private GameObject iciclePrefab;

        [SerializeField] public float spawnDuration;
        [SerializeField] private float spawnFrequency;
        [SerializeField] private float spawnRangeX;
        [SerializeField] private float spawnRangeY;
        private float StartTime;
        
        void Start()
        {
            StartTime = Time.time;
            StartCoroutine(IcicleSpawner_CO());
        }

        //Spawn icicles at a given interval for a given duration. 
        private IEnumerator IcicleSpawner_CO()
        {
            yield return new WaitForSeconds(spawnFrequency);
            //Determine the spawn position and rotation. 
            float xPos = Random.Range(-spawnRangeX, spawnRangeX);
            float yPos = Random.Range(-spawnRangeY, spawnRangeY);
            Vector2 pos = new Vector2(transform.position.x + xPos, transform.position.y + yPos);
            
            //Rotation
            var test = Random.rotation;
            float zRotation = Random.Range(0, 180);
            Quaternion rotation = new Quaternion(0, 0, test.z, 0);
            
            var obj = Instantiate(iciclePrefab, pos, rotation);
            //Swap the spawned object's sprite to a different one. 
            obj.GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
            obj.GetComponent<Rigidbody2D>().gravityScale = Random.Range(.2f, 1f);

            if (StartTime + spawnDuration > Time.time)
            {
                StartCoroutine(IcicleSpawner_CO());
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
