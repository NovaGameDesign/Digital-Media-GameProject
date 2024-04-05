using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Player/Air Attack", fileName = "Player_Attack_Air")]
    public class PlayerAttackAirborne : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponRange;
        [SerializeField] private Vector2 weaponOffset; 
        

        public override void Activate(GameObject holder)
        {
            holder.GetComponent<PlayerCombatSystem>()?.HandleBasicAttack(weaponOffset, weaponRange);
        }
    }
}
