using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace FirstSlice.Dialogs
{
    public class DialogVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Image portrait = null;
        [SerializeField]
        private TMP_Text speaker = null;
        [SerializeField]
        private TMP_Text message = null;

        public UnityEvent OnDialogStarted = null;
        public UnityEvent OnDialogFinished = null;
        public UnityEvent OnDialogAnimationStarted = null;
        public UnityEvent OnDialogAnimationFinished = null;

        private Dialog dialog = null;
        private string speakerName = "";
        private string line = "";

        private bool dialogLineCompletelyDisplayed = false;

        public void DialogStarted(Dialog dialog)
        {
            ResetUI();

            this.dialog = dialog;
            OnDialogStarted?.Invoke();
        }

        public void DialogFinished()
        {
            ResetUI();
            OnDialogFinished?.Invoke();
        }

        private void ResetUI()
        {
            speakerName = "";
            line = "";
            speaker.text = "";
            message.text = "";
            dialog = null;
        }

        public void DialogLineChanged(DialogLine dialogLine)
        {
            StopAllCoroutines();

            speakerName = dialogLine.differentSpeaker ? dialogLine.speakerName : dialog.speakerName;
            line = dialogLine.line;

            dialogLineCompletelyDisplayed = false;
            OnDialogAnimationStarted?.Invoke();
            DisplaySpeakerName();
        }

        private void DisplaySpeakerName()
        {
            if (speaker.text != speakerName)
            {
                StartCoroutine(DisplayTextDelayed(speaker, speakerName, DisplayDialogLine));
            }
            else
            {
                DisplayDialogLine();
            }
        }

        private void DisplayDialogLine()
        {
            StartCoroutine(DisplayTextDelayed(message, line, DialogLineCompleted));
        }

        private readonly string invisiblePrefix = "<color=#0000>";
        private readonly string invisibleSuffix = "</color>";

        private string currentText = "";

        private IEnumerator DisplayTextDelayed(TMP_Text tmp, string text, UnityAction onFinished)
        {
            float time = 0f;
            int index = 0;

            currentText = text;
            tmp.text = "";

            while (index < text.Length)
            {
                string textThisFrame = text[0..(index + 1)];

                if (index < text.Length - 1)
                {
                    textThisFrame += invisiblePrefix + text[(index + 1)..] + invisibleSuffix;
                }

                tmp.text = textThisFrame;

                char character = text[index];
                float timeNeeded = GetCharacterTime(character);
                while (timeNeeded > time)
                {
                    yield return null;
                    time += Time.deltaTime;
                }

                time -= timeNeeded;
                index++;
            }

            tmp.text = text;
            OnDialogAnimationFinished?.Invoke();

            onFinished?.Invoke();
        }

        public void CompleteLine()
        {
            if (dialogLineCompletelyDisplayed)
            {
                return;
            }

            StopAllCoroutines();
            dialogLineCompletelyDisplayed = true;
            speaker.text = speakerName;
            message.text = line;
            OnDialogAnimationFinished?.Invoke();
        }

        private void DialogLineCompleted()
        {
            OnDialogAnimationFinished?.Invoke();
        }

        [SerializeField]
        private float LetterTime = 0.05f;
        [SerializeField]
        private float SpaceTime = 0.025f;
        [SerializeField]
        private float CommaTime = 0.075f;
        [SerializeField]
        private float DotTime = 0.4f;

        [SerializeField]
        private float randomFactor = 0.25f;

        private float GetCharacterTime(char character)
        {
            float characterTime = character switch
            {
                char letter when char.IsLetter(letter) => LetterTime,
                ' ' => SpaceTime,
                ',' => CommaTime,
                '.' => DotTime,
                '!' => DotTime,
                '?' => DotTime,

                _ => LetterTime
            };

            characterTime += characterTime * Random.Range(-randomFactor, randomFactor);

            return characterTime;
        }
    }
}