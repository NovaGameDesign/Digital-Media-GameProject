using DigitalMedia.AI.ActionNodes;
using UnityEngine;

namespace DigitalMedia
{
    public class WaitNode : ActionNode
    {
        public float waitDuration = 1;
        private float startTime;
        protected override void OnsStart()
        {
            startTime = Time.time;
        }

        protected override void OnStop()
        {
           
        }

        protected override State OnUpdate()
        {
            if (Time.time - startTime > waitDuration)
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}
