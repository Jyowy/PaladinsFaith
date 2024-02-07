using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
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
