using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class ButtonActivation : Activation
    {
        public void Activate()
        {
            Active = true;
        }

        public void Consume()
        {
            Active = false;
        }
    }
}
