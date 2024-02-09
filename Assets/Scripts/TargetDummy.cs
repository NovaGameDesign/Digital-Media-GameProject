using UnityEngine;

namespace DigitalMedia
{
    public class TargetDummy : EnemyBase
    {
        private CircleCollider2D attackRange;

        private void Start()
        {
            attackRange = GetComponents<CircleCollider2D>()[1]; // We should only ever have two circle colliders so we can just assume it is the second one. 
        }

        public override void Attack()
        {
            attackRange.gameObject.SetActive(true);
        
        }
    }
}
