using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Input
{
    public abstract class PlayerInputDataProvider : MonoBehaviour
    {
        public abstract PlayerInputData UpdateAndGetPlayerInputData();
    }
}