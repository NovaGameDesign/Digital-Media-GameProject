using System.Collections;
using System.Collections.Generic;
using DigitalMedia.AI;
using UnityEngine;

namespace DigitalMedia
{
    public class RootNode : Node
    {
        [HideInInspector] public Node child;
        protected override void OnsStart()
        {
            
        }

        protected override void OnStop()
        {
           
        }

        protected override State OnUpdate()
        {
            return child.Update();
        }
        
        public override Node Clone()
        {
            RootNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}
