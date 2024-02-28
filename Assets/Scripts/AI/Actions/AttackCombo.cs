using System.Collections;
using System.Collections.Generic;
using DigitalMedia;
using DigitalMedia.Core;
using UnityEngine;
using TheKiwiCoder;


[System.Serializable]
public class AttackCombo : ActionNode
{
    private bool justAttacked;
    private EnemyCoreCombat combatReference;
    public string attackToTry = "Attack_Combo";
    public NodeProperty<GameObject> playerReference = new NodeProperty<GameObject> { defaultValue = null };
    protected override void OnStart()
    {
        combatReference = context.gameObject.GetComponent<EnemyCoreCombat>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (playerReference.Value.GetComponent<PlayerStats>() != null)
        {
            if(playerReference.Value.GetComponent<PlayerStats>().health <= 0) return State.Success;
        }
        //Maybe check the current animation state's name and see if it matches our attack to try. There may be a situation where the attackCombo tries a new attack but it indicates it already is attacking if that makes sense.
        if (combatReference.currentState == DigitalMedia.Core.State.Attacking) 
        {
            return State.Running;
        }
        else if (justAttacked && combatReference.currentState != DigitalMedia.Core.State.Attacking)
        {
            justAttacked = false;
            return State.Success;
        }
        else
        {
            if(context.agent.isOnNavMesh) context.agent.isStopped = true;
  
            combatReference.TryToAttack(attackToTry);
            justAttacked = true; 
            //context.animator.GetCurrentAnimatorStateInfo()
        }
        return State.Success;
    }
}
