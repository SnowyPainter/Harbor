using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.ML
{
    public static class Property
    {
        [Serializable]
        public enum Emotion
        {
            Angry,
            Good,
            Sad,
            Laugh,
        }
        [Serializable]
        public struct PosNegPercent
        {
            public float Positive;
            public float Negative;

            public PosNegPercent(float pp,float np)
            {
                Positive = pp;
                Negative = np;
            }
            public bool IsValid() // sum != 100 -> false
            {
                return Positive + Negative == 100f;
            }
            
        }
    }
}
