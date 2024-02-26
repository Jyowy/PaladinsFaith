using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Input
{
    public abstract class Activation
    {
        public bool Active { get; protected set; } = false;
    }
}
