using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Dialogs
{
    [Serializable]
    public struct DialogLine
    {
        public string name;
        [TextArea]
        public string line;
        public NextDialogType next;
        [ShowIf("next", NextDialogType.JumpTo)]
        public string nextDialogName;
        [HideIf("next", NextDialogType.EndDialog)]
        public bool hasAnswers;
        [ShowIf("hasAnswers")]
        public List<DialogAnswer> answers;
        public bool differentSpeaker;
        [ShowIf("differentSpeaker")]
        public string speakerName;
    }

    public enum NextDialogType
    {
        Continue,
        JumpTo,
        EndDialog
    }

    [Serializable]
    public struct DialogAnswer
    {
        public string line;
        public NextDialogType next;
        [ShowIf("next", NextDialogType.JumpTo)]
        public string nextDialogName;
    }

    [CreateAssetMenu()]
    public class Dialog : ScriptableObject
    {
        public string dialogName = "";
        public string speakerName = "";
        public List<DialogLine> lines = new List<DialogLine>();
    }
}