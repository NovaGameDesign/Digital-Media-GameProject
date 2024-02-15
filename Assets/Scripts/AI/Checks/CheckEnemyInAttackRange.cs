using UnityEngine;

namespace DigitalMedia.AI.Checks
{
    public class CheckEnemyInAttackRange : Node
    {
        private static int _enemyLayermask = 1 << 6;
        private float _attackrange;

        private Transform _tranform;
        // private Animator _animator;

        public CheckEnemyInAttackRange(Transform tranform, float attackrange)
        {
            _tranform = tranform;
            _attackrange = attackrange;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                state = NodeState.FAILURE;
                return state;
            }
        
            Transform target = (Transform)t;
            if (target == null)
            {
                ClearData("target");
            }    
            if (Vector3.Distance(_tranform.position, target.position) < _attackrange)
            {
                // Set animator values so that attacking occurs and walking stops. 


                state = NodeState.SUCCESS; 
                return state;
            }

            state = NodeState.FAILURE; return state;
        }
    }
}
