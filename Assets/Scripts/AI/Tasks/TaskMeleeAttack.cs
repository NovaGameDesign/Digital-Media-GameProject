using UnityEngine;
using DigitalMedia.Core;

namespace DigitalMedia.AI.Tasks
{
    public class TaskMeleeAttack : Node
    {
        private Transform _lastTarget;
        private StatsComponent targetStats;
        private EnemyBase _enemyBase;

        private float _attackTime = 1f;
        private float _attackCounter = 0f;
        private bool enemyIsDead;

        public TaskMeleeAttack(Transform transform, EnemyBase enemyBase)
        {
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
                //_enemyBase.MeleeAttack();
                /*if(targetStats.health <= 0)
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
