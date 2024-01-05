using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    [Serializable]
    public struct DialogLine
    {
        [ReadOnly]
        public int id;
        public string name;

        public string line;
        public NextDialogType next;
        [ShowIf("next", NextDialogType.JumpTo)]
        public int nextDialog;
        [HideIf("next", NextDialogType.EndDialog)]
        [ReadOnly]
        public string nextDialogName;
        [HideIf("next", NextDialogType.EndDialog)]
        public bool hasAnswers;
        [ShowIf("hasAnswers")]
        [ListDrawerSettings(CustomAddFunction = "AddAnswer")]
        public List<DialogAnswer> answers;

        public DialogLine(int id)
        {
            this.id = id;
            name = $"Dialog {id}";
            line = $"Dialog Line N# {id}.";
            next = NextDialogType.Continue;
            nextDialogName = "";
            nextDialog = 0;
            hasAnswers = false;
            answers = new List<DialogAnswer>();
        }
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
        public int nextDialog;
    }

    [CreateAssetMenu()]
    public class Dialog : ScriptableObject
    {
        [ListDrawerSettings(CustomAddFunction = "AddLine", CustomRemoveIndexFunction = "RemoveLine")]
        [OnValueChanged("ListChanged")]
        public List<DialogLine> lines = new List<DialogLine>();

        [SerializeField, HideInInspector]
        private int nextId = 0;

#if UNITY_EDITOR
        private void AddLine()
        {
            Debug.Log($"AddLine!");
            int id = GetNextId();
            DialogLine line = new DialogLine(id);
            lines.Add(line);
        }

        private int GetNextId()
        {
            return nextId++;
        }

        private void RemoveLine(int index)
        {
            Debug.Log($"RemoveLine! {index}");
            lines.RemoveAt(index);
        }

        private void ListChanged()
        {
            Debug.Log($"ListChanged!");
        }
#endif
    }
}
