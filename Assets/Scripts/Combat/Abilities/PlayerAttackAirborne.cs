using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Player/Air Attack", fileName = "Player_Attack_Air")]
    public class PlayerAttackAirborne : AbilityBase
    {
        [Header("Attack Range")]
        [SerializeField] private Vector2 weaponRange;
        [SerializeField] private Vector2 weaponOffset; 
        
        [Header("Lightning Element Info")]
        [SerializeField] private GameObject lightningSlash;
        [SerializeField] public float slashDamage;
        [SerializeField] private Vector2 spawnLocation;
        
        public override void Activate(GameObject holder)
        {
            PlayerCombatSystem combatSystem = holder.GetComponent<PlayerCombatSystem>();
            combatSystem?.HandleBasicAttack(weaponOffset, weaponRange);
            
            
            if(combatSystem.currentElement is Elements.Lightning)
            {
                var obj = Instantiate(lightningSlash, holder.transform.position, holder.transform.rotation);
                obj.transform.parent = holder.transform;
                obj.transform.localPosition = spawnLocation;
            }
        }
    }
}
