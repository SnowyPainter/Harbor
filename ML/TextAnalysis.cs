using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.ML
{
    
    public static class TextAnalysis
    {
        public static Property.Emotion GetEmotionFromText(string text)
        {
            return Property.Emotion.Angry;
        }
        public static Property.PosNegPercent GetPosNegPercentFromText(string text)
        {
            var p = new Property.PosNegPercent { Positive = 100f, Negative = 0f };

            if (p.Positive + p.Negative != 100) throw new Exception("Numeric err");

            return p;
        }
    }
}
