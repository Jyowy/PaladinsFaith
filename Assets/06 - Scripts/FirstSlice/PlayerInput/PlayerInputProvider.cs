using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.PlayerInput
{
    public abstract class PlayerInputDataProvider : MonoBehaviour
    {
        public abstract PlayerInputData GetPlayerInputData();
    }
}