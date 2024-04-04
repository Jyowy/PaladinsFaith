using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public Object Context { get; private set; } = null;
    public string Name { get; private set; } = null;

    public float CurrentTime { get; private set; } = 0f;
    public float CurrentProgress { get; private set; } = 0f;
    public float Duration { get; private set; } = 0f;

    public bool IsCompleted() => CurrentTime >= Duration;

    private readonly bool ignoreTimeScale = false;
    private readonly System.Action<float> onProgress = null;
    private readonly System.Action onFinished = null;

    private readonly float startTime = 0f;

    public Timer(Object context, string name, float duration, System.Action<float> onProgress, System.Action onFinished, bool ignoreTimeScale)
    {
        Context = context;
        Name = name;
        Duration = duration;
        this.onProgress = onProgress;
        this.onFinished = onFinished;
        this.ignoreTimeScale = ignoreTimeScale;

        startTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
        SetTime(0f);
    }

    public bool IsTimer(Object context, string name)
    {
        bool sameContext = Context == context;
        bool sameName = Name == name;
        bool doesntSpecifyName = name == null;

        bool isTimer = (sameContext && sameName)
            || (sameContext && doesntSpecifyName);

        return isTimer;
    }

    public void UpdateTime()
    {
        if (IsCompleted())
        {
            return;
        }

        float time = ignoreTimeScale ? Time.unscaledTime : Time.time;
        float newTime = time - startTime;
        SetTime(newTime);
    }

    private void SetTime(float time)
    {
        CurrentTime = Mathf.Clamp(time, 0f, Duration);
        CurrentProgress = Duration > 0f
            ? Mathf.Clamp01(CurrentTime / Duration)
            : 1f;

        onProgress?.Invoke(CurrentProgress);

        if (IsCompleted())
        {
            onFinished?.Invoke();
        }
    }

    public void Stop()
    {
        CurrentTime = Duration;
    }
}