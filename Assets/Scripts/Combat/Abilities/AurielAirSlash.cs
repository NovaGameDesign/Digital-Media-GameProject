using UnityEngine;

namespace DigitalMedia.Combat.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Auriel/Air Slash", fileName = "Auriel_AirSlash")]
    public class AurielAirSlash : AbilityBase
    {
        private Collider2D[] overlapping;

        [SerializeField] private Vector2 weaponOffset; 
        [SerializeField] private Vector2 weaponRange;

        [Header("Slash Info")]
        public GameObject airSlash;
        public float slashDamage;
        [SerializeField] private Vector2 spawnLocation;
        
        public override void Activate(GameObject holder)
        {
            holder.GetComponent<EnemyCoreCombat>().HandleBasicAttack(weaponOffset, weaponRange);
            var obj = Instantiate(airSlash, spawnLocation, holder.transform.rotation);
            obj.transform.parent = holder.transform;
            obj.transform.localPosition = spawnLocation;
            obj.transform.parent = null;
            //Play audio 
            //Play Effects 

        }
    }
}
