using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FloatCheck : DecoratorNode
{
    public NodeProperty<float> ValueToCheck = new NodeProperty<float> { defaultValue = 0.1f };

    public float TargetValue = 0;
    
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (ValueToCheck.Value == TargetValue)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
