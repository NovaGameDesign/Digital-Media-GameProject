using System.Collections;
using System.Collections.Generic;
using DigitalMedia.AI;
using UnityEngine;

namespace DigitalMedia
{
    public class SequenceNode : CompositeNode
    {
        private int current;
        
        protected override void OnsStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            var child = children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    current++;
                    break;
            }

            return current == children.Count ? State.Success : State.Running;
        }
    }
}
