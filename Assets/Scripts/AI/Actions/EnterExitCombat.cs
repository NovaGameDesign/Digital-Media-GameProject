using DigitalMedia;
using DigitalMedia.Core;
using TheKiwiCoder;
using UnityEngine;


[System.Serializable]
public class EnterExitCombat : ActionNode
{
    public AudioClip clipToPlayOnEnter;
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
            if (shouldEnter)
            {
                var musicManager = GameObject.Find("Music");
                
                if (context.gameObject.GetComponent<AurielStats>().currentLives == 1) return State.Success;
                
                musicManager.GetComponent<MusicManager>().PlayMusic(clipToPlayOnEnter);
            }
            else
            {
                GameObject.Find("Music").GetComponent<MusicManager>().PlayDefaultSong();
            }
            return State.Success;
        }
        return State.Failure;
    }
}

