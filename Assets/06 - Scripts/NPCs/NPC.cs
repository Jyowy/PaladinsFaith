using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Characters;
using PaladinsFaith.Dialogs;
using UnityEngine.Events;

namespace PaladinsFaith.NPC
{
    public class NPC : Character, Interactable
    {
        [SerializeField]
        private string characterName = "NPC";
        [SerializeField]
        private Dialog dialog = null;

        public UnityEvent onInRangeOfInteraction = null;
        public UnityEvent onOutRangeOfInteraction = null;
        public UnityEvent<Dialog> onTalkStarted = null;
        public UnityEvent<Dialog> onTalkFinished = null;

        public void InRangeOfInteraction()
        {
            onInRangeOfInteraction?.Invoke();
        }

        public void OutRangeOfInteraction()
        {
            onOutRangeOfInteraction?.Invoke();
        }

        public void Interact()
        {
            Talk();
        }

        private void Talk()
        {
            Debug.Log($"Talk!");

            DialogPlayer.StartDialog(dialog, FinishTalk);
            onTalkStarted?.Invoke(dialog);
        }

        private void FinishTalk()
        {
            onTalkFinished?.Invoke(dialog);
        }
    }
}
