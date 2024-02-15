using UnityEngine;
using UnityEngine.AI;

namespace DigitalMedia.AI.Tasks
{
    public class TaskMoveToPlayer : Node
    {
        private Transform _transform;
        private PlayerController _player;
        private NavMeshAgent _agent;

        public TaskMoveToPlayer(Transform transform, PlayerController player)
        {
            _transform = transform;
            _player = player;
        }

        public override NodeState Evaluate()
        {
            if(Vector3.Distance(_transform.position, _player.transform.position) < 4f)
            {
                _agent.isStopped = true;
                return NodeState.SUCCESS;
            }
            if(_agent == null) 
            {
                _agent = _transform.transform.GetComponent<NavMeshAgent>();
            }

            if( _player != null )
            {
                _agent.isStopped = false;
                _agent.SetDestination(_player.transform.position);
                return NodeState.RUNNING;

            }

            return NodeState.FAILURE;
        }
    }
}
