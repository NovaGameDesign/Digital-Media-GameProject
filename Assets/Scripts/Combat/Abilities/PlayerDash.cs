using DigitalMedia.Core;
using DigitalMedia.Misc;
using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Player/Dash", fileName = "Player_Dash")]
    public class PlayerDash : AbilityBase
    {
        [Header("Dash Information ")] [SerializeField]
        private LayerMask layersToCheck;
        [SerializeField] private float dashDistance;

        public float dashTime; 

        [SerializeField] public float dashSpeed;

        public float dashTimeLeft;

        public float lastImageXpos;

        public float distanceBetweenTwoImages;

        private RaycastHit2D hit;
        
        public override void Activate(GameObject holder)
        {
           Debug.Log($"We tried to dash and have {dashTimeLeft}");
           //Set dashing to true, check for cooldown, etc. 
           //dashTimeLeft = dashTime;
           holder.GetComponent<PlayerCombatSystem>().InitiateStateChange(State.Dashing);
           
           
            
           hit = Physics2D.Raycast(holder.transform.position, holder.transform.right, dashDistance, layersToCheck);
           if (hit.collider != null)
           {
               Debug.Log($"We hit a wall or object, the hit location was {hit.point}");
               holder.transform.position = Vector2.Lerp(holder.transform.position, hit.transform.position, Time.deltaTime);
           }
           else
           {
               holder.transform.position = Vector2.Lerp(holder.transform.position, new Vector2(holder.transform.position.x + dashDistance, holder.transform.position.y), Time.deltaTime);
           }
           
           
           /*PlayerAfterImagePool.Instance.GetFromPool();
           lastImageXpos = holder.transform.position.x;*/
        }
       
    }
}