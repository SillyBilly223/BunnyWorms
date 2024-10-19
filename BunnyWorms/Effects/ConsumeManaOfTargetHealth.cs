using System;
using System.Collections.Generic;
using System.Text;

namespace BunnyWorms.Effects
{
    public class ConsumeManaOfTargetHealth : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].HasUnit)
                {
                    JumpAnimationInformation jumpInfo = stats.GenerateUnitJumpInformation(caster.ID, caster.IsUnitCharacter);
                    string manaConsumedSound = stats.audioController.manaConsumedSound;
                    exitAmount += stats.MainManaBar.ConsumeAmountMana(targets[i].Unit.HealthColor, entryVariable, jumpInfo, manaConsumedSound);
                }     
            }
            return exitAmount > 0;
        }
    }
}
