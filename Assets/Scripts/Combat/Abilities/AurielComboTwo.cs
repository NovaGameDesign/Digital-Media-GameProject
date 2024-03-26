using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Auriel/Second Combo", fileName = "Auriel_Combo-Two")]
    public class AurielComboTwo : AbilityBase
    {
        private Collider2D[] overlapping;

        [SerializeField] private Vector2 [] weaponOffset; 
        [SerializeField] private Vector2 [] weaponRange;
        
        private int currentAttackIndex;
        public override void Activate(GameObject holder)
        {
            
            holder.GetComponent<EnemyCoreCombat>().HandleBasicAttack(weaponOffset[currentAttackIndex], weaponRange[currentAttackIndex]);
            //Play audio 
            //Play Effects 
            
        }
    }
}
