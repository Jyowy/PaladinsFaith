using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Effects
{
    public static class GameObjectExtensions
    {
        public static bool HasLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return (gameObject.layer & layerMask.value) == 1;
        }
    }
}