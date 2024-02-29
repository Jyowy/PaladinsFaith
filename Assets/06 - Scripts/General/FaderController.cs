using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith
{
    public class FaderController : MonoBehaviour
    {
        private enum State
        {
            None,
            FadingIn,
            FadedIn,
            FadingOut,
            FadedOut,
        }

        private enum Type
        {
            FadeIn,
            FadeOut
        }

        [SerializeField]
        private CanvasGroup canvasGroup = null;
        [SerializeField]
        private Type startMode = Type.FadeOut;
        [SerializeField]
        private bool disableOnFadeOut = true;

        [SerializeField, Range(0f, 1f)]
        private float fadeInValue = 1f;
        [SerializeField]
        private float fadeInDefaultTime = 1f;
        [SerializeField]
        private bool useFadeInCurve = false;
        [SerializeField, ShowIf(nameof(useFadeInCurve))]
        private AnimationCurve fadeInCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        [SerializeField, Range(0f, 1f)]
        private float fadeOutValue = 0f;
        [SerializeField]
        private float fadeOutDefaultTime = 1f;
        [SerializeField]
        private bool useFadeOutCurve = false;
        [SerializeField, ShowIf(nameof(useFadeOutCurve))]
        private AnimationCurve fadeOutCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

        public UnityEvent onFadeAnimationFinished = null;

        [ShowInInspector, ReadOnly]
        private State state = State.None;
        [ShowInInspector, ReadOnly]
        private float animationTime = 0f;
        [ShowInInspector, ReadOnly]
        private float animationDuration = 0f;

        private void Awake()
        {
            if (state != State.None)
            {
                return;
            }

            SetStartState();
        }

        private void SetStartState()
        {
            if (startMode == Type.FadeIn)
            {
                FadeIn_Instant();
            }
            else
            {
                FadeOut_Instant();
            }
        }

        [Button]
        public void FadeIn_Instant()
        {
            FadeIn(0f);
        }

        [Button]
        public void FadeIn()
        {
            FadeIn(fadeInDefaultTime);
        }

        [Button]
        public void FadeIn(float duration)
        {
            if (disableOnFadeOut)
            {
                gameObject.SetActive(true);
            }
            StartCoroutine(FadeIn_Implementation(duration));
        }

        private IEnumerator FadeIn_Implementation(float duration)
        {
            if (duration <= 0f)
            {
                ClearAnimationData();
                state = State.FadedIn;
                float alpha = GetFinalFadeInAlpha();
                SetAlpha(alpha);
                yield break;
            }

            StartAnimation(Type.FadeIn, duration);

            while (animationTime < duration)
            {
                yield return null;

                if (state != State.FadingIn)
                {
                    yield break;
                }

                animationTime += Time.deltaTime;
                float alpha = GetFadeInAlpha(animationTime);
                SetAlpha(alpha);
            }

            state = State.FadedIn;

            EndAnimation();
        }

        [Button]
        public void FadeOut_Instant()
        {
            FadeOut(0f);
        }

        [Button]
        public void FadeOut()
        {
            FadeOut(fadeOutDefaultTime);
        }

        [Button]
        public void FadeOut(float duration)
        {
            StartCoroutine(FadeOut_Implementation(duration));
        }

        private IEnumerator FadeOut_Implementation(float duration)
        {
            if (duration <= 0f)
            {
                ClearAnimationData();
                state = State.FadedOut;
                float alpha = GetFinalFadeOutAlpha();
                SetAlpha(alpha);
                yield break;
            }

            Debug.Log($"FadeOut called");
            StartAnimation(Type.FadeOut, duration);

            while (animationTime < duration)
            {
                yield return null;

                if (state != State.FadingOut)
                {
                    yield break;
                }

                animationTime += Time.deltaTime;
                float alpha = GetFadeOutAlpha(animationTime);
                SetAlpha(alpha);
            }

            state = State.FadedOut;
            if (disableOnFadeOut)
            {
                Debug.Log($"Faded out so disabled the object");
                gameObject.SetActive(false);
            }

            EndAnimation();
        }

        private void StartAnimation(Type type, float newDuration)
        {
            if (type == Type.FadeIn)
            {
                if (state == State.FadingIn)
                {
                    Debug.LogError($"Trying to start fade-in animation while another fade-in animation is playing.");
                }
                else if (state == State.FadedIn)
                {
                    Debug.LogError($"Trying to start fade-in animation while it's already faded in.");
                }
                else if (state == State.FadingOut)
                {
                    StopCoroutine(nameof(FadeOut_Implementation));
                    RevertTime(newDuration);
                    state = State.FadingIn;
                }
                else
                {
                    state = State.FadingIn;
                    animationTime = 0f;
                }
            }
            else if (type == Type.FadeOut)
            {
                if (state == State.FadingOut)
                {
                    Debug.LogError($"Trying to start fade-out animation while another fade-out animation is playing.");
                }
                else if (state == State.FadedOut)
                {
                    Debug.LogError($"Trying to start fade-out animation while it's already faded out.");
                }
                else if (state == State.FadingIn)
                {
                    StopCoroutine(nameof(FadeIn_Implementation));
                    RevertTime(newDuration);
                    state = State.FadingOut;
                }
                else
                {
                    state = State.FadingOut;
                    animationTime = 0f;
                }
            }

            animationDuration = newDuration;
        }

        private float GetFadeInAlpha(float time)
        {
            float progress = Mathf.Clamp01(time / animationDuration);
            float alphaFactor = useFadeInCurve
                ? fadeInCurve.Evaluate(progress)
                : GetLinearFadeInAlpha(progress);
            float alpha = Mathf.Lerp(fadeOutValue, fadeInValue, alphaFactor);
            return alpha;
        }

        private float GetFinalFadeInAlpha()
        {
            float alphaFactor = useFadeInCurve
                ? fadeInCurve.Evaluate(1f)
                : GetLinearFadeInAlpha(1f);
            float alpha = Mathf.Lerp(fadeOutValue, fadeInValue, alphaFactor);
            return alpha;
        }

        private float GetLinearFadeInAlpha(float progress)
        {
            float alpha = progress;
            return alpha;
        }

        private float GetFadeOutAlpha(float time)
        {
            float progress = Mathf.Clamp01(time / animationDuration);
            float alphaFactor = useFadeOutCurve
                ? fadeOutCurve.Evaluate(progress)
                : GetLinearFadeOutAlpha(progress);
            float alpha = Mathf.Lerp(fadeOutValue, fadeInValue, alphaFactor);
            return alpha;
        }

        private float GetFinalFadeOutAlpha()
        {
            float alphaFactor = useFadeOutCurve
                ? fadeOutCurve.Evaluate(1f)
                : GetLinearFadeOutAlpha(1f);
            float alpha = Mathf.Lerp(fadeOutValue, fadeInValue, alphaFactor);
            return alpha;
        }

        private float GetLinearFadeOutAlpha(float progress)
        {
            float alpha = 1f - progress;
            return alpha;
        }

        private void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }

        private void StopCurrentAnimation()
        {
            StopAllCoroutines();
        }

        private void RevertTime(float newDuration)
        {
            float contraryProgress = Mathf.Clamp01(animationTime / animationDuration);
            float progress = 1f - contraryProgress;
            animationTime = newDuration * progress;
        }

        private void ClearAnimationData()
        {
            StopCurrentAnimation();
            animationDuration = 0f;
            animationTime = 0f;
        }

        private void EndAnimation()
        {
            animationDuration = 0f;
            animationTime = 0f;

            onFadeAnimationFinished?.Invoke();
        }
    }
}