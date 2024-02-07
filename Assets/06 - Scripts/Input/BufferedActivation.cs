using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public class BufferedActivation : Activation
    {
        public float RemainingTime { get; private set; } = 0f;

        public void Activate(float bufferTime)
        {
            Active = true;
            RemainingTime = bufferTime;
        }

        public void UpdateTime(float dt)
        {
            if (!Active)
            {
                return;
            }

            RemainingTime = Mathf.Max(RemainingTime - dt, 0f);
            if (RemainingTime == 0f)
            {
                Finish();
            }
        }

        public void Finish()
        {
            Active = false;
            RemainingTime = 0f;
        }
    }
}