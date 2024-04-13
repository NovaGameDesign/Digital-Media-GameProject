using DigitalMedia.Core;
using DigitalMedia.Misc;
using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Player/Dash", fileName = "Player_Dash")]
    public class PlayerDash : AbilityBase
    {
        [Header("Dash Distance ")]
        [SerializeField] private float dashDistance;

        public float dashTime; 

        [SerializeField] public float dashSpeed;

        public float dashTimeLeft;

        public float lastImageXpos;

        public float distanceBetweenTwoImages;
        
        public override void Activate(GameObject holder)
        {
            Debug.Log($"We tried to dash and have {dashTimeLeft}");
           //Set dashing to true, check for cooldown, etc. 
           dashTimeLeft = dashTime;
           holder.GetComponent<PlayerCombatSystem>().InitiateStateChange(State.Dashing);
           
           
           PlayerAfterImagePool.Instance.GetFromPool();
           lastImageXpos = holder.transform.position.x;
        }
    }
}