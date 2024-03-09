using PaladinsFaith.Player;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Characters
{
    public class Character : MonoBehaviour
    {
        [System.Flags]
        public enum AlteredStates
        {
            None = 0,
            Pushed
        }
        [SerializeField]
        protected CharacterMoveModule moveModule = null;

        [SerializeField]
        protected ContinuousResource stamina = new ContinuousResource(100f);

        [ShowInInspector]
        public AlteredStates State { get; protected set; } = 0;

        protected virtual void Awake()
        {
            if (moveModule != null)
            {
                moveModule.SetStamina(stamina);
            }
        }

        protected virtual void Update()
        {
            float dt = Time.deltaTime;
            stamina.Update(dt);
        }

        public void MoveTo(Vector3 position)
        {
            moveModule.MoveTo(position);
        }

        public void Push(Vector3 direction, float strength, float duration)
        {
            moveModule.Push(direction, strength, duration);
        }

        public void SimplePushBack()
        {
            moveModule.SimplePushBack();
        }
    }
}
