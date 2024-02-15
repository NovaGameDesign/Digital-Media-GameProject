using System.Collections.Generic;

namespace DigitalMedia.AI
{
    /// <summary>
    /// Selectors cancel execution after a a node succeeds in its execution. If they fail, they continue down the list of nodes. 
    /// </summary>
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }


        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;                        
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;

                }
            }
            state = NodeState.FAILURE;
            return state;
        }

    }

}