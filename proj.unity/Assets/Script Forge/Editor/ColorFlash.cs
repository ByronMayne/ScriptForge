using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace ScirptForge
{
    public class GUIColorFlash : BaseAnimValue<Color>
    {
        protected GUIColorFlash(Color value) : base(value)
        {
            
        }

        protected GUIColorFlash(Color value, UnityAction callback) : base(value, callback)
        {

        }

        protected override Color GetValue()
        {

            float sinLerp = Mathf.Sin(lerpPosition * 2f);
            sinLerp += 1;
            sinLerp /= 2.0f;
            return Color.Lerp(Color.white, value, sinLerp);
        }
    }
}
