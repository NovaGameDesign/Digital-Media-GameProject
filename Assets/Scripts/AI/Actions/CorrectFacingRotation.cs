using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CorrectFacingRotation : ActionNode
{
    [Tooltip("Target Position")]
    public NodeProperty<Vector2> targetPosition = new NodeProperty<Vector2> { defaultValue = Vector2.zero };

    public bool defaultFacingRight = false;
    
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.transform.position.x > targetPosition.Value.x)
        {
            float yRotation = defaultFacingRight ? 180 : 0;
            // rotation = new Quaternion(0, yRotation, 0, 0);
            context.transform.rotation = new Quaternion(0, yRotation, 0, 0);
        }
        else if (context.transform.position.x < targetPosition.Value.x)
        {
            float yRotation = defaultFacingRight ? 0 : 180;
            // rotation = new Quaternion(0, yRotation, 0, 0);
            context.transform.rotation = new Quaternion(0, yRotation, 0, 0);
        }
        else
        {
            return State.Failure;
        }
        
        return State.Success;
    }
}
