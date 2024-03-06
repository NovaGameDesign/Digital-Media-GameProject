using UnityEngine;

namespace DigitalMedia.Misc
{
    public class ParallaxBackground : MonoBehaviour
    {
        private float _startingPos; //This is starting position of the sprites.
        private float _lengthOfSprite;    //This is the length of the sprites.
        public float AmountOfParallax;  //This is amount of parallax scroll. 
        public UnityEngine.Camera MainCamera;   //Reference of the camera.
    
        private void Start()
        {
            //Getting the starting X position of sprite.
            _startingPos = transform.position.x;    
            //Getting the length of the sprites.
            _lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    
        private void Update()
        {
            Vector3 cameraPosition = MainCamera.transform.position;
            float temp = cameraPosition.x * (1 - AmountOfParallax);
            float Distance = cameraPosition.x * AmountOfParallax;

            transform.position = new Vector3(_startingPos + Distance, transform.position.y, 0);

            if (temp > _startingPos + _lengthOfSprite) _startingPos += _lengthOfSprite;
            else if (temp < _startingPos - _lengthOfSprite) _startingPos -= _lengthOfSprite;
        }
    }
}