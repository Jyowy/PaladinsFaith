using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.PlayerInput
{
    public abstract class PlayerInputDataProvider : MonoBehaviour
    {
        public abstract PlayerInputData GetPlayerInputData();
    }
}