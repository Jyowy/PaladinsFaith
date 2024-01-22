using FirstSlice.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace FirstSlice
{
    public class DialogSignalReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable _, INotification notification, object __)
        {
            Debug.Log($"Hola! {notification}");
            if (notification is DialogSignalEmitter dialogEmitter)
            {
                Debug.Log($"Yes!");
                switch (dialogEmitter.type)
                {
                    case DialogSignalEmitter.DialogType.Start:
                        StartNewDialog(dialogEmitter.dialog);
                        break;

                    case DialogSignalEmitter.DialogType.Next:
                        NextDialog();
                        break;

                    case DialogSignalEmitter.DialogType.End:
                        EndDialog();
                        break;
                }
            }
        }

        private void StartNewDialog(Dialog dialog)
        {
            Debug.Log($"StartNewDialog");
            DialogPlayer.StartDialog(dialog);
        }

        private void NextDialog()
        {
            DialogPlayer.CompleteLine();
        }

        private void EndDialog()
        {
            DialogPlayer.EndDialog();
        }
    }
}
