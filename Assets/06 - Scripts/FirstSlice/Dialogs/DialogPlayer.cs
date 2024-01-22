using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstSlice.Dialogs
{
    public class DialogPlayer : HiddenSingleton<DialogPlayer>
    {
        public UnityEvent<Dialog> onDialogStarted = null;
        public UnityEvent<DialogLine> onDialogLineChanged = null;
        public UnityEvent onDialogFinished = null;

        public bool IsPlaying => CurrentDialog != null;
        public Dialog CurrentDialog { get; private set; } = null;

        private int currentDialogLineIndex = -1;
        public DialogLine CurrentDialogLine { get; private set; } = new DialogLine();
        private UnityAction dialogFinishedCallback = null;

        public static void StartDialog(Dialog dialog, UnityAction onDialogFinished = null)
        {
            Instance.StartDialog_Internal(dialog, onDialogFinished);
        }

        private void StartDialog_Internal(Dialog dialog, UnityAction onDialogFinished)
        {
            if (dialog == null)
            {
                Debug.LogError($"PlayDialog: given dialog is null!");
                return;
            }

            EndDialog_Internal();

            CurrentDialog = dialog;
            dialogFinishedCallback = onDialogFinished;
            onDialogStarted?.Invoke(dialog);

            PlayFirstLine();
        }

        public static void EndDialog()
        {
            Instance.EndDialog_Internal();
        }

        private void EndDialog_Internal()
        {
            if (CurrentDialog == null)
            {
                return;
            }

            onDialogFinished?.Invoke();
            dialogFinishedCallback?.Invoke();
            ClearDialog();
        }

        private void ClearDialog()
        {
            CurrentDialog = null;
            currentDialogLineIndex = -1;
            CurrentDialogLine = new DialogLine();
            dialogFinishedCallback = null;
        }

        private void PlayFirstLine()
        {
            ShowLine(0);
        }

        private void ShowLine(string lineName)
        {
            int lineIndex = GetDialogLineIndexByName(lineName);
            ShowLine(lineIndex);
        }

        private int GetDialogLineIndexByName(string lineName)
        {
            return CurrentDialog.lines.FindIndex(line => line.name.Equals(lineName));
        }

        private void ShowLine(int lineIndex)
        {
            if (IsLineIndexOutOfRange(lineIndex))
            {
                Debug.LogError($"ShowLine of dialog '{CurrentDialog.dialogName}': lineIndex {lineIndex} is out of range (0, {CurrentDialog.lines.Count - 1})");
                return;
            }

            currentDialogLineIndex = lineIndex;
            CurrentDialogLine = CurrentDialog.lines[lineIndex];
            onDialogLineChanged?.Invoke(CurrentDialogLine);
        }

        private bool IsLineIndexOutOfRange(int lineIndex)
        {
            return lineIndex < 0 || lineIndex >= CurrentDialog.lines.Count;
        }

        public static void CompleteLine()
        {
            Instance.CompleteLine_Internal();
        }

        private void CompleteLine_Internal()
        {
            if (CurrentDialogLine.hasAnswers)
            {
                Debug.LogError($"Line '{CurrentDialogLine.name}': can't complete line, choose an answer!");
                return;
            }

            ProcessNextDialog(CurrentDialogLine.next, CurrentDialogLine.nextDialogName);
        }

        private void ProcessNextDialog(NextDialogType nextType, string nextDialogName)
        {
            switch (nextType)
            {
                case NextDialogType.EndDialog:
                    EndDialog_Internal();
                    break;

                case NextDialogType.JumpTo:
                    ShowLine(nextDialogName);
                    break;

                case NextDialogType.Continue:
                default:
                    int nextLine = currentDialogLineIndex + 1;
                    ShowLine(nextLine);
                    break;
            }
        }

        public void ChooseAnswer(int answerIndex)
        {
            if (!CurrentDialogLine.hasAnswers
                || CurrentDialogLine.answers.Count == 0)
            {
                Debug.LogError($"Choose Answer for line '{CurrentDialogLine.name}': can't choose answer, it doens't have!");
                return;
            }
            else if (IsAnswerIndexOutOfRange(answerIndex))
            {
                Debug.LogError($"Choose Answer for line '{CurrentDialogLine.name}': answer index {answerIndex} is out of range (0, {CurrentDialogLine.answers.Count - 1})");
                return;
            }

            DialogAnswer dialogAnswer = CurrentDialogLine.answers[answerIndex];
            ProcessNextDialog(dialogAnswer.next, dialogAnswer.nextDialogName);
        }

        private bool IsAnswerIndexOutOfRange(int answerIndex)
        {
            return answerIndex < 0 || answerIndex >= CurrentDialogLine.answers.Count;
        }
    }
}