using UnityEngine;

namespace DigitalMedia.Misc
{
    public class ParallaxBackground : MonoBehaviour
    {
        private float _startingPos; //This is starting position of the sprites.
        private float _lengthOfSprite;    //This is the length of the sprites.
        public float AmountOfParallax;  //This is amount of parallax scroll. 
        public float AmountOfParallaxY;
        public UnityEngine.Camera MainCamera;   //Reference of the camera.
        public bool followY; 
        private void Start()
        {
            //Getting the starting X position of sprite.
            _startingPos = transform.position.x;    
            //Getting the length of the sprites.
            _lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    
        private void FixedUpdate()
        {   
            Vector3 cameraPosition = MainCamera.transform.position;
            float temp = cameraPosition.x * (1 - AmountOfParallax);
            float Distance = cameraPosition.x * AmountOfParallax;

            var tempY = cameraPosition.y - 8;
            float yPos = followY ? tempY * AmountOfParallaxY : transform.position.y;
            transform.position = new Vector3(_startingPos + Distance, yPos, 0);

            if (temp > _startingPos + _lengthOfSprite) _startingPos += _lengthOfSprite;
            else if (temp < _startingPos - _lengthOfSprite) _startingPos -= _lengthOfSprite;
        }
    }
}