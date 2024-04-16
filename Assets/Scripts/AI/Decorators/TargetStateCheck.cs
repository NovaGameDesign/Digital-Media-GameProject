using System.Collections;
using System.Collections.Generic;
using DigitalMedia.Combat;
using DigitalMedia.Core;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Serialization;

[System.Serializable]
public class TargetStateCheck : DecoratorNode
{
    //public NodeProperty<DigitalMedia.Core.State> valueTocheck  = new NodeProperty<DigitalMedia.Core.State> { defaultValue = DigitalMedia.Core.State.None};

    [FormerlySerializedAs("StatesToCancelExecution")] public List<DigitalMedia.Core.State> statesToCancelExecution = new List<DigitalMedia.Core.State>();
    
    public NodeProperty<GameObject> TargetRef  = new NodeProperty<GameObject> { defaultValue = null};


    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {


        if (!statesToCancelExecution.Contains(TargetRef.Value.GetComponent<CoreCombatSystem>().currentState))
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

        if (context.gameObject.GetComponent<EnemyCoreCombat>().currentState == DigitalMedia.Core.State.Attacking)
        {
            context.gameObject.GetComponent<EnemyCoreCombat>().InitiateStateChange(DigitalMedia.Core.State.Idle);
        }
        
        Debug.Log("The target was in a state that prevented further execution of the Behavior Tree");
        context.gameObject.GetComponent<Animator>().Play("Idle");
        return State.Failure;
    }
}
