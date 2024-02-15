using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.AI.Checks
{
    public class CheckEnemyInFOVRange : Node
    {
        private Transform _transform;
        private float targetHealth = -1;
        private Transform target = null;
        private string _targetTag;
        private float _fov;

        private static int _enemyLayerMask = 1 << 6;
    

        public CheckEnemyInFOVRange(Transform transform, string targetTag, float fov)
        {
            _transform = transform;
            _targetTag = targetTag;
            _fov = fov;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null )
            {
                Collider[] colliders = Physics.OverlapSphere(_transform.position, _fov);
           
                if(colliders.Length > 0 )
                {
                    // Debug.Log("Detected something");
                    for (int i = 0; i < colliders.Length; i++)
                    {
                  
                        GameObject temp = colliders[i].gameObject;
                        if (temp.tag == _targetTag)
                        {
                            //Debug.Log("Detected an enemy of some kind!");
                            if (targetHealth > temp.GetComponent<StatsComponent>().health)
                            {
                                targetHealth = temp.GetComponent<StatsComponent>().health;
                                target = temp.transform;
                            }
                            else if (targetHealth == -1)
                            {
                                targetHealth = temp.GetComponent<StatsComponent>().health;
                                target = temp.transform;
                            }
                        }
                    }
                    if(target != null) 
                    {
                        parent.parent.SetData("target", target.transform); //used to be colliders[0].transform if I need to change it back. 
                        state = NodeState.SUCCESS;
                        return state;
                    }
                
                }

                state = NodeState.FAILURE;
                return state;
            }
            else if (t != null)
            {
                var temp = (Transform)GetData("target");
                if (Vector3.Distance(temp.position, _transform.position) > _fov)
                {
                    ClearData("target");
                    state = NodeState.FAILURE; return state;
                }
            }

            state = NodeState.SUCCESS;
            return state;
        }
    }
}
