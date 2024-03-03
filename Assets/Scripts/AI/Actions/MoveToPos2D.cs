using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System.IO;
using UnityEngine.AI;

namespace TheKiwiCoder {

    [System.Serializable]
    public class MoveToPos2D : ActionNode {

        [Tooltip("How fast to move")]
        public NodeProperty<float> speed = new NodeProperty<float> { defaultValue = 5.0f };

        [Tooltip("Stop within this distance of the target")]
        public NodeProperty<float> stoppingDistance = new NodeProperty<float> { defaultValue = 0.1f };

        [Tooltip("Updates the agents rotation along the path")]
        public NodeProperty<bool> updateRotation = new NodeProperty<bool> { defaultValue = true };

        [Tooltip("Maximum acceleration when following the path")]
        public NodeProperty<float> acceleration = new NodeProperty<float> { defaultValue = 40.0f };

        [Tooltip("Returns success when the remaining distance is less than this amount")]
        public NodeProperty<float> tolerance = new NodeProperty<float> { defaultValue = 1.0f };

        [Tooltip("Target Position")]
        public NodeProperty<Vector2> targetPosition = new NodeProperty<Vector2> { defaultValue = Vector2.zero };

        protected override void OnStart() {
            if (context.agent != null) {
                context.agent.stoppingDistance = stoppingDistance.Value;
                context.agent.speed = speed.Value;
                context.agent.updateRotation = updateRotation.Value;
                context.agent.acceleration = acceleration.Value;
                context.agent.isStopped = false;
            }
        }

        protected override void OnStop() {
            if (context.agent.pathPending) {
                context.agent.ResetPath();
            }

            if (context.agent.remainingDistance > tolerance.Value) {
                context.agent.isStopped = true;
            }
        }

        protected override State OnUpdate() {
            if (context.agent == null) {
                Debug.Log($"Game object {context.gameObject.name} is missing NavMeshAgent component");
                return State.Failure;
            }

            if (!context.agent.enabled) {
                Debug.Log($"NavMeshAgent component on {context.gameObject.name} was disabled");
                return State.Failure;
            }

            //Theoretically we should do a raycast down to get the actual surface but whatever, we can get an arbitrary point by just setting the value to lower than the player. 
            Vector3 correctedPosition = new Vector3(targetPosition.Value.x, targetPosition.Value.y - 2, 0);
            context.agent.SetDestination(correctedPosition);
            
            //Update the rotation to match the movement direction. 
            if (context.agent.velocity.x > 0)
            {
                var rotation = context.transform.rotation;
                rotation = new Quaternion(rotation.x, 180, rotation.z, rotation.w);
                context.transform.rotation = rotation;
            }
            else
            {
                var rotation = context.transform.rotation;
                rotation = new Quaternion(rotation.x, 0, rotation.z, rotation.w);
                context.transform.rotation = rotation;
            }
            
            
            if (context.agent.pathPending) {
                return State.Running;
            }

            if (context.agent.remainingDistance < tolerance.Value) {
                return State.Success;
            }

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
                return State.Failure;
            }
            return State.Running;
        }

        public override void OnDrawGizmos() {
            var agent = context.agent;
            var transform = context.transform;

            // Current velocity
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);

            // Desired velocity
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);

            // Current path
            Gizmos.color = Color.black;
            var agentPath = agent.path;
            Vector3 prevCorner = transform.position;
            foreach (var corner in agentPath.corners) {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
