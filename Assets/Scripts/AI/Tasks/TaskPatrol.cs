using UnityEngine;
using UnityEngine.AI;

namespace DigitalMedia.AI.Tasks
{
    public class TaskPatrol : Node
    {
        private Transform _transform;
        private Transform[] _waypoints;
        private int _currentWaypointIndex = 0;

        private float _waitTime = 1f; //In Seconds
        private float _waitCounter = 0f;
        private bool _waiting = false;
        protected bool agentFound;
        protected NavMeshAgent agent = null;

        public TaskPatrol(Transform transform, Transform[] waypoints)
        {
            _transform = transform;
            _waypoints = waypoints;
        }

        public override NodeState Evaluate()
        {
            if (!agentFound)
            {
                agent = _transform.gameObject.GetComponent<NavMeshAgent>();
                agentFound = true;
            }
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if(_waitCounter >= _waitTime)
                    _waiting = false;

            }
            else
            {
            
                Transform wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.2f)
                {
                    _transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + +1) % _waypoints.Length;
                }
                else
                {
                    agent.SetDestination(wp.position);
                    //_transform.position = Vector3.MoveTowards(_transform.position, wp.position, GuardBT.speed * Time.deltaTime);
                    //_transform.LookAt(wp.position);
                }
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
