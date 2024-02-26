using PaladinsFaith;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timers : HiddenSingleton<Timers>
{
    [ShowInInspector, ReadOnly]
    private List<Timer> OngoingTimers = null;
    [ShowInInspector, ReadOnly]
    private List<Timer> PausedTimers = null;

    public static void StartGameTimer(Object context, float duration, System.Action onFinished)
    {
        StartGameTimer(context, duration, null, onFinished);
    }

    public static void StartGameTimer(Object context, string name, float duration, System.Action onFinished)
    {
        StartGameTimer(context, name, duration, null, onFinished);
    }

    public static void StartGameTimer(Object context, float duration, System.Action<float> onProgress, System.Action onFinished)
    {
        StartGameTimer(context, null, duration, onProgress, onFinished);
    }

    public static void StartGameTimer(Object context, string name, float duration, System.Action<float> onProgress, System.Action onFinished)
    {
        StartTimer(context, name, duration, onProgress, onFinished, false);
    }

    public static void StartGlobalTimer(Object context, float duration, System.Action onFinished)
    {
        StartGlobalTimer(context, duration, null, onFinished);
    }

    public static void StartGlobalTimer(Object context, string name, float duration, System.Action onFinished)
    {
        StartGlobalTimer(context, name, duration, null, onFinished);
    }

    public static void StartGlobalTimer(Object context, float duration, System.Action<float> onProgress, System.Action onFinished)
    {
        StartGlobalTimer(context, null, duration, onProgress, onFinished);
    }

    public static void StartGlobalTimer(Object context, string name, float duration, System.Action<float> onProgress, System.Action onFinished)
    {
        StartTimer(context, name, duration, onProgress, onFinished, true);
    }

    private static void StartTimer(Object context, string name, float duration, System.Action<float> onProgress, System.Action onFinished, bool ignoreTimeScale)
    {
        CheckInstance();
        Instance.StartTimer_Instanced(context, name, duration, onProgress, onFinished, ignoreTimeScale);
    }

    private static void CheckInstance()
    {
        if (Instance != null
            || !Application.isPlaying)
        {
            return;
        }

        GameObject timersObject = new GameObject("Timers");
        Timers timers = timersObject.AddComponent<Timers>();
        timers.enabled = false;
        timers.OngoingTimers = new List<Timer>();
        timers.PausedTimers = new List<Timer>();
}

    private void StartTimer_Instanced(Object context, string name, float duration, System.Action<float> onProgress, System.Action onFinished, bool ignoreTimeScale)
    {
        if (context == null)
        {
            return;
        }

        StopTimer_Instanced(context, name);
        Timer newTimer = new Timer(context, name, duration, onProgress, onFinished, ignoreTimeScale);
        OngoingTimers.Add(newTimer);
        CheckComponentEnabled();
    }

    private void CheckComponentEnabled()
    {
        enabled = OngoingTimers.Count > 0;
    }

    public static void StopTimer(Object context)
    {
        StopTimer(context, null);
    }

    public static void StopTimer(string name)
    {
        StopTimer(null, name);
    }

    public static void StopTimer(Object context, string name)
    {
        if (Instance == null)
        {
            return;
        }

        Instance.StopTimer_Instanced(context, name);
    }

    private void StopTimer_Instanced(Object context, string name)
    {
        StopTimers(OngoingTimers, context, name);
        StopTimers(PausedTimers, context, name);
    }

    private void StopTimers(List<Timer> timers, Object context, string name)
    {
        int index = 0;
        while (index < timers.Count)
        {
            Timer timedAction = timers[index];
            if (timedAction.IsTimer(context, name))
            {
                timers.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
    }

    public static void PauseTimer(Object context)
    {
        PauseTimer(context, null);
    }

    public static void PauseTimer(string name)
    {
        PauseTimer(null, name);
    }

    public static void PauseTimer(Object context, string name)
    {
        if (Instance == null)
        {
            return;
        }

        Instance.PauseTimer_Instanced(context, name);
    }

    private void PauseTimer_Instanced(Object context, string name)
    {
        int index = 0;
        while (index < OngoingTimers.Count)
        {
            Timer timedAction = OngoingTimers[index];
            if (timedAction.IsTimer(context, name))
            {
                OngoingTimers.RemoveAt(index);
                PausedTimers.Add(timedAction);
            }
            else
            {
                index++;
            }
        }

        CheckComponentEnabled();
    }

    public static void ResumeTimer(Object context)
    {
        ResumeTimer(context, null);
    }

    public static void ResumeTimer(string name)
    {
        ResumeTimer(null, name);
    }

    public static void ResumeTimer(Object context, string name)
    {
        if (Instance == null)
        {
            return;
        }

        Instance.ResumeTimer_Instanced(context, name);
    }

    private void ResumeTimer_Instanced(Object context, string name)
    {
        int index = 0;
        while (index < OngoingTimers.Count)
        {
            Timer timedAction = PausedTimers[index];
            if (timedAction.IsTimer(context, name))
            {
                PausedTimers.RemoveAt(index);
                OngoingTimers.Add(timedAction);
            }
            else
            {
                index++;
            }
        }

        CheckComponentEnabled();
    }

    private void Update()
    {
        UpdateUngoingTimers();
    }

    private void UpdateUngoingTimers()
    {
        CleanUnnecessaryTimers(OngoingTimers);
        if (!enabled)
        {
            return;
        }

        int index = 0;
        while (index < OngoingTimers.Count)
        {
            Timer timer = OngoingTimers[index];
            timer.UpdateTime();

            if (timer.IsCompleted())
            {
                OngoingTimers.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
    }

    private void CleanUnnecessaryTimers(List<Timer> timers)
    {
        int index = 0;
        while (index < timers.Count)
        {
            Timer timedAction = timers[index];
            if (IsTimerUnnecessary(timedAction))
            {
                timers.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }

        CheckComponentEnabled();
    }

    private bool IsTimerUnnecessary(Timer timer)
    {
        return timer.Context == null
            || timer.IsCompleted();
    }
}
