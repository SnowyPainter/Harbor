using System;
using System.Collections.Generic;
using System.Text;

namespace Harbor.ML.Model
{
    public interface ITextAnalyzer
    {
        Property.Emotion GetEmotionFromText(string text);
        Property.Emotion GetEmotionByPNP(Property.PosNegPercent pnp);
        Property.PosNegPercent GetPNPFromText(string text);
    }

    public class DefaultTextAnalysis : ITextAnalyzer
    {
        public Property.Emotion GetEmotionFromText(string text)
        {
            return Property.Emotion.Angry;
        }
        //Customing by user
        public Property.Emotion GetEmotionByPNP(Property.PosNegPercent pnp)
        {
            return Property.Emotion.Good;
        }
        public Property.PosNegPercent GetPNPFromText(string text)
        {
            var p = new Property.PosNegPercent { Positive = 100f, Negative = 0f };

            if (p.Positive + p.Negative != 100) throw new Exception("Numeric err");

            return p;
        }
    }
}
