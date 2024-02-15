using System.Collections.Generic;

namespace DigitalMedia.AI
{
    /// <summary>
    /// Sequences continue execution even after a node succeeds in its execution. This means that if a node has three childern and child 1 succeed child 2 will execute. However, if child 2 fails then the execution is canceled. 
    /// </summary>
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }


        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false; 
            
            foreach(Node node in children) 
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:                        
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;

                }
            }
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}

