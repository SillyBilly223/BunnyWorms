using BunnyWorms.BunnyWormComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace BunnyWorms.Effects
{
    public class SpawnBunnyWormAnywhereEffect : EffectSO
    {
        public bool givesExperience;

        [CombatIDsEnumRef]
        public string _spawnTypeID = "Spawn_Basic";

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            for (int i = 0; i < entryVariable; i++)
            {
                CombatManager.Instance.AddSubAction(new SpawnEnemyAction(BunnyWormEnemy.GetBunnyWorm(), -1, givesExperience, trySpawnAnyways: false, _spawnTypeID));
            }

            exitAmount = entryVariable;
            return true;
        }
    }
}
