using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
     [CreateAssetMenu(menuName = "Abilities/Player/Combo One", fileName = "Player_Combo_One")]
    public class PlayerAttackComboOne : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponOffset; 
        [SerializeField] private Vector2 weaponRange; 
        
        public override void Activate(GameObject holder)
        {
            holder.GetComponent<PlayerCombatSystem>()?.HandleBasicAttack(weaponOffset, weaponRange);
            holder.GetComponent<PlayerCombatSystem>().currentAttackIndex++;
            holder.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.25f, 0));
            //Play audio 
            //Play Effects 
        }
    }
}