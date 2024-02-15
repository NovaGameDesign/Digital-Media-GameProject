using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.AI.Tasks
{
    public class TaskRangedAttack : Node
    {
        private Transform _lastTarget;
        private Transform _transform;
        private StatsComponent targetStats;
        private EnemyBase _enemyBase;

        private float _attackTime = 1f;
        private float _attackCounter = 0f;
        private bool enemyIsDead;

        public TaskRangedAttack(Transform transform, EnemyBase enemyBase)
        {
            _transform = transform;
            _enemyBase = enemyBase;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
       
            if(target != _lastTarget)
            {
                targetStats = target.transform.GetComponentInChildren<StatsComponent>();
                _lastTarget = target;   
            }

            _attackCounter += Time.deltaTime;
            if(_attackCounter >= _attackTime ) 
            {
                _transform.LookAt(target);
                //_enemyBase.RangedAttack();
                /*if (targetStats.health <= 0)
                {
                    Tree.setRunTree();
                    ClearData("target");
                    //reset animator
                }
                else
                    _attackCounter = 0f;*/
            }
        
            state = NodeState.RUNNING; 
            return state;
        }
    }
}
