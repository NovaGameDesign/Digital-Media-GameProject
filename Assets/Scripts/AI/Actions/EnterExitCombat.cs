using DigitalMedia.Core;
using TheKiwiCoder;


[System.Serializable]
public class EnterExitCombat : ActionNode
{
    public bool shouldEnter;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (context.gameObject.GetComponent<StatsComponent>() != null)
        {
            context.gameObject.GetComponent<StatsComponent>().inCombat = shouldEnter;
            return State.Success;
        }
        return State.Failure;
    }
}

