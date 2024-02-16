using DigitalMedia.AI.Decorators;

namespace DigitalMedia
{
    public class RepeatNode : DecoratorNode
    {
        protected override void OnsStart()
        {
            
        }

        protected override void OnStop()
        {
          
        }

        protected override State OnUpdate()
        {
            //Can customize to loop a certain amount of times or only while succeeding, may come back to this later. 
            child.Update();
            return State.Running;
        }
    }
}
