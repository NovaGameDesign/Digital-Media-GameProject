using UnityEngine;
using UnityEngine.AI;

namespace DigitalMedia.AI.Tasks
{
    public class TaskGoToTarget : Node
    {
        private Transform _transform;
        protected NavMeshAgent agent = null;
        private bool agentFound;

        public TaskGoToTarget(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (!agentFound)
            {
                agent = _transform.gameObject.GetComponent<NavMeshAgent>();
                agentFound = true;
            }

            if (Vector3.Distance(_transform.position, target.position) > 0.01f)
            {
                agent.SetDestination(target.position);
                // _transform.position = Vector3.MoveTowards(_transform.position, target.position, GuardBT.speed * Time.deltaTime);
                // _transform.LookAt(target.position);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
