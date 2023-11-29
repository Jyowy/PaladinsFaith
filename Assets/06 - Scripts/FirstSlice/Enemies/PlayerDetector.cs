using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstSlice
{
    [RequireComponent(typeof(Collider))]
    public class PlayerDetector : MonoBehaviour
    {
        public UnityEvent<GameObject> OnPlayerFound = null;
        public UnityEvent OnPlayerLost = null;

        private void OnTriggerEnter(Collider other)
        {
            //if (other.CompareTag("Player"))
            {
                OnPlayerFound?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //if (other.CompareTag("Player"))
            {
                OnPlayerLost?.Invoke();
            }
        }
    }
}
