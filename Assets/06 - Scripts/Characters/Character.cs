using PaladinsFaith.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField]
        protected CharacterMoveModule moveModule = null;

        [SerializeField]
        protected ContinuousResource stamina = new ContinuousResource(100f);

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
    }
}
