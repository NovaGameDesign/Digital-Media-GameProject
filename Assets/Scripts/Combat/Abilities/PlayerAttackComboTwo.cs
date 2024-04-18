using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Player/Combo Two", fileName = "Player_Combo_Two")]
    public class PlayerAttackComboTwo : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponOffset; 
        [SerializeField] private Vector2 weaponRange; 
        
        [Header("Fire Weapon Range")]
        [SerializeField] private Vector2 fireWeaponOffset; 
        [SerializeField] private Vector2 fireWeaponRange; 
        
        public override void Activate(GameObject holder)
        {
            if (holder.GetComponent<PlayerCombatSystem>().currentElement == Elements.Fire)
            {
                holder.GetComponent<PlayerCombatSystem>()?.HandleBasicAttack(fireWeaponOffset, fireWeaponRange);
                
            }
            else
            {
                holder.GetComponent<PlayerCombatSystem>()?.HandleBasicAttack(weaponOffset, weaponRange);

            }

            holder.GetComponent<PlayerCombatSystem>().currentAttackIndex++;

        }
    }
}