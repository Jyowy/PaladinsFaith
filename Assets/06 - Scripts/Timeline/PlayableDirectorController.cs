using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableDirectorController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director = null;

    public void PlayFromStart(PlayableAsset asset)
    {
        director.playableAsset = asset;
        director.time = 0;
        director.Evaluate();

        director.Play();
    }

    public void Enable()
    {
        director.enabled = true;
    }

    public void Disable()
    {
        director.enabled = false;
    }
}
