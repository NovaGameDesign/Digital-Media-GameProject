using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Player/Combo Three", fileName = "Player_Combo_Three")]
    public class PlayerAttackComboThree : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponOffset; 
        [SerializeField] private Vector2 weaponRange; 
        
        public override void Activate(GameObject holder)
        {
            holder.GetComponent<CombatSystem>()?.HandleBasicAttack(weaponOffset, weaponRange);
            holder.GetComponent<CombatSystem>().currentAttackIndex = 0;
            //Play audio 
            //Play Effects 
        }
    }
}