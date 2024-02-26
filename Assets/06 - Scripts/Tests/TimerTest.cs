using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    [SerializeField]
    private bool useContext = false;
    [SerializeField]
    private GameObject context = null;
    [SerializeField]
    private bool useName = false;
    [SerializeField]
    private string timerName = "Name";

    [SerializeField]
    private float duration = 2f;
    [SerializeField]
    private bool ignoreTimeScale = false;

    [SerializeField]
    private bool useProgressAction = false;

    [ShowInInspector]
    private bool debugFlag = false;

    [Button]
    public void StartTimer()
    {
        Object context = useContext ? this.context : null;
        string name = useName ? timerName : null;
        System.Action<float> onProgress = useProgressAction ? TimerProgress : null;

        Debug.Log($"Timer started {duration}");
        debugFlag = false;

        if (ignoreTimeScale)
        {
            Timers.StartGlobalTimer(context, name, duration, onProgress, TimerFinished);
        }
        else
        {
            Timers.StartGameTimer(context, name, duration, onProgress, TimerFinished);
        }
    }

    private void TimerProgress(float progress)
    {
        Debug.Log($"Timer progress {(Mathf.Clamp(Mathf.RoundToInt(progress * 100f), 0f, 100))}%");
    }

    private void TimerFinished()
    {
        Debug.Log($"Timer finished");
        debugFlag = true;
    }

    [Button]
    private void DestroyContext()
    {
        Destroy(context);
    }
}
