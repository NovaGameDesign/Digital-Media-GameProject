using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IntCheck : DecoratorNode
{
    public NodeProperty<int> ValueToCheck = new NodeProperty<int> { defaultValue = 0 };

    public int TargetValue = 0;
    
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
