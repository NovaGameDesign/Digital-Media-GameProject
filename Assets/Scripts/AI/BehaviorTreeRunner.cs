using DigitalMedia.AI;
using DigitalMedia.AI.ActionNodes;
using UnityEngine;

namespace DigitalMedia
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        public BehaviorTree tree;
        // Start is called before the first frame update
        void Start()
        {
            tree = tree.Clone();
        }

        // Update is called once per frame
        void Update()
        {
            tree.Update();
        }
    }
}
