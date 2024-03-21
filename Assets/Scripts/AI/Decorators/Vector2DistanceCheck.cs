using TheKiwiCoder;
using UnityEngine;

[System.Serializable]
public class Vector2DistanceCheck : DecoratorNode
{
    public NodeProperty<Vector2> ValueToCheck = new NodeProperty<Vector2> { defaultValue = new Vector2(0,0) };

    /// <summary>
    /// Distance that we are ok with proceeding in execution,.
    /// </summary>
    public float acceptanceRange = 0;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        float distance = Vector2.Distance(context.gameObject.transform.position, ValueToCheck.Value);
        //Debug.Log("The distance was "+distance);
        if (distance <= acceptanceRange)
        {
            switch (child.Update()) {
                case State.Running:
                    return State.Running;
                    break;
                case State.Failure:
                    return State.Failure;
                    break;
                case State.Success:
                    return State.Success;
                    break;  
            }
        }
        return State.Failure;

    }
}

