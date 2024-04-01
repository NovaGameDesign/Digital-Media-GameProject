using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Target-Dummy/Main Attack", fileName = "Dummy_Attack")]
    public class DummyBasicAttack : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponRange; 

        [SerializeField] private Vector2 weaponOffset; 

        public override void Activate(GameObject holder)
        {
            holder.GetComponent<EnemyCoreCombat>()?.HandleBasicAttack(weaponOffset, weaponRange);
            //Play audio 
            //Play Effects 
        }
    }
}
