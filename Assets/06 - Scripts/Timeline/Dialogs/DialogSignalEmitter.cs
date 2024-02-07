using PaladinsFaith.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace PaladinsFaith
{
    public class DialogSignalEmitter : SignalEmitter
    {
        public enum DialogType
        {
            Start,
            Next,
            End
        }

        public Dialog dialog = null;
        public DialogType type = DialogType.Next;
    }
}
