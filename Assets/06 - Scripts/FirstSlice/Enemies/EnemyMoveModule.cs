using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public abstract class EnemyMoveModule : MonoBehaviour
    {
        public abstract void MoveTo(Vector3 position);

        public abstract void Stop();

        public abstract void LookAt(Vector3 position);
    }
}
