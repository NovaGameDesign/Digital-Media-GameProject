using UnityEngine;

namespace DigitalMedia.AI.Tasks
{
    public class TaskLookAtTarget : Node
    {
        private Transform _transform;
        public TaskLookAtTarget(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != null)
            {
                _transform.LookAt(target);
                return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }
    }
}
