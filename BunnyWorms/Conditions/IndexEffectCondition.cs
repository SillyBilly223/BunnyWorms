using System;
using System.Collections.Generic;
using System.Text;

namespace BunnyWorms.Condition
{
    public class IndexEffectConditon : EffectConditionSO
    {
        public bool wasSuccessful = true;

        public int EffectIndex = 0;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            if (EffectIndex < 0 || effects.Length - 1 < EffectIndex) return false;
            return effects[EffectIndex].EffectSuccess == wasSuccessful;
        }
    }
}
