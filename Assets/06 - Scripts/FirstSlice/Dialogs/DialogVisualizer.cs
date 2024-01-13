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

        private Dialog dialog = null;
        private string speakerName = "";
        private string line = "";

        private bool lineDisplayedCompletely = true;

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

            lineDisplayedCompletely = false;
            speakerName = dialogLine.differentSpeaker ? dialogLine.speakerName : dialog.speakerName;
            line = dialogLine.line;

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
            StartCoroutine(DisplayTextDelayed(message, line, null));
        }

        private IEnumerator DisplayTextDelayed(TMP_Text tmp, string text, UnityAction onFinished)
        {
            float time = 0f;
            int index = 0;

            tmp.text = "";

            while (index < text.Length)
            {
                tmp.text = text.Substring(0, index + 1);

                float timeNeeded = GetCharacterTime(text[index]);
                while (timeNeeded > time)
                {
                    yield return null;
                    time += Time.deltaTime;
                }

                time -= timeNeeded;
                index++;
            }

            tmp.text = text;
            onFinished?.Invoke();
        }


        [ShowInInspector]
        private readonly float LetterTime = 0.04f;
        [ShowInInspector]
        private readonly float SpaceTime = 0.08f;
        [ShowInInspector]
        private readonly float CommaTime = 0.4f;
        [ShowInInspector]
        private readonly float DotTime = 0.8f;

        private float GetCharacterTime(char character)
        {
            return character switch
            {
                char letter when char.IsLetter(letter) => LetterTime,
                ' ' => SpaceTime,
                ',' => CommaTime,
                '.' => DotTime,
                '!' => DotTime,
                '?' => DotTime,

                _ => LetterTime
            };
        }
    }
}