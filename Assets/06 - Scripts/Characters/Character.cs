using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField]
        protected CharacterMoveModule moveModule = null;

        public void MoveTo(Vector3 position)
        {
            moveModule.MoveTo(position);
        }
    }
}
