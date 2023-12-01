using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FirstSlice
{
    public class UIGauge : Gauge
    {
        [SerializeField]
        private Image fillingImage = null;

        protected override void ValueChanged()
        {
            base.ValueChanged();

            fillingImage.fillAmount = Progress;
        }
    }
}
