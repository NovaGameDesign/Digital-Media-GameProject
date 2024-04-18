using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Player/Combo Three", fileName = "Player_Combo_Three")]
    public class PlayerAttackComboThree : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponOffset; 
        [SerializeField] private Vector2 weaponRange; 
        
        [Header("Fire Weapon Range")]
        [SerializeField] private Vector2 fireWeaponOffset; 
        [SerializeField] private Vector2 fireWeaponRange; 
        
        [Header("Lightning Element Info")]
        [SerializeField] private GameObject lightningSlash;
        [SerializeField] public float slashDamage;
        [SerializeField] private Vector2 spawnLocation;
        
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

            if(combatSystem.currentElement is Elements.Lightning)
            {
                var obj = Instantiate(lightningSlash, holder.transform.position, holder.transform.rotation);
                obj.transform.parent = holder.transform;
                obj.transform.localPosition = spawnLocation;
            }
        }
    }
}