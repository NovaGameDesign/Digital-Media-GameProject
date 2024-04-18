using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
     [CreateAssetMenu(menuName = "Abilities/Player/Combo One", fileName = "Player_Combo_One")]
    public class PlayerAttackComboOne : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponOffset; 
        [SerializeField] private Vector2 weaponRange;
        
        [Header("Fire Weapon Range")]
        [SerializeField] private Vector2 fireWeaponOffset; 
        [SerializeField] private Vector2 fireWeaponRange; 
        
        public override void Activate(GameObject holder)
        {
            PlayerCombatSystem combatSystem = holder.GetComponent<PlayerCombatSystem>();
            if (holder.GetComponent<PlayerCombatSystem>().currentElement == Elements.Fire)
            {
                combatSystem.HandleBasicAttack(fireWeaponOffset, fireWeaponRange);
            }
            else
            {
                combatSystem.HandleBasicAttack(weaponOffset, weaponRange);
            }
            combatSystem.currentAttackIndex++;
            
        }
    }
}