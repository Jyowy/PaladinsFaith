using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Input
{
    public class ButtonHoldingActivation : Activation
    {
        private float startTime = 0f;
        public float HoldingTime => Active ? Time.time - startTime : 0f;

        public void Activate()
        {
            startTime = Time.time;
            Active = true;
        }

        public void Consume()
        {
            Active = false;
        }
    }
}
