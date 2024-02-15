using System.Collections.Generic;
using DigitalMedia.AI.Checks;
using DigitalMedia.AI.Tasks;

namespace DigitalMedia.AI.BTs
{
    public class AllyBT : Tree
    {

        public static float speed = 2f;
        public static float fovRange = 5f;
        public static float attackRange = 5f;
        public PlayerController player;
    

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>         
            {            
                new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(transform, attackRange),
                    new TaskLookAtTarget(transform),
                    new TaskRangedAttack(transform, this.GetComponent<EnemyBase>()),
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInFOVRange(player.transform, "Enemy", fovRange),
                
                }),
                new TaskMoveToPlayer(transform, player),
            });         
          
            return root;
        }
    }
}
