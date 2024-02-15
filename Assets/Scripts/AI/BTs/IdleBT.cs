using System.Collections.Generic;
using DigitalMedia.AI.Checks;
using DigitalMedia.AI.Tasks;

namespace DigitalMedia.AI.BTs
{
    public class IdleBT : Tree
    {

        public static float speed = 2f;
        public static float fovRange = 10f;
        public static float attackRange = 5f;
    

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>         //1.1 ------------- Order of operation
            {
                new Sequence(new List<Node>                 //2.1
                {
                    new CheckEnemyInAttackRange(transform, attackRange), //3.1.1
                    new TaskRangedAttack(transform, S_EnemyBase),              //3.1.2
                }),
                new Sequence(new List<Node>                 //2.2
                {
                    new CheckEnemyInFOVRange(transform, "Player", fovRange),    //3.2.1
                    new TaskGoToTarget(transform),          //3.2.2
                }),            
            });         
          
            return root;
        }
    }
}
