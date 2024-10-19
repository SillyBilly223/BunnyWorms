using System;
using System.Collections.Generic;
using System.Text;
using Random = UnityEngine.Random;

namespace BunnyWorms.Effects
{
    public class ReturnRandomEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = Random.value > 0.5f? 1 : 0;
            return exitAmount > 0;
        }
    }
}
