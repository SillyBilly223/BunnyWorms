using BunnyWorms.BunnyWormComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Random = UnityEngine.Random;

namespace BunnyWorms.Actions
{
    public class SpawnBunnyWormsAction : CombatAction
    {
        public override IEnumerator Execute(CombatStats stats)
        {
            CombatSlot[] Slots = stats.combatSlots.EnemySlots;

            for (int i = 0; i < Slots.Length; i++)
                if (!Slots[i].HasUnit && Random.Range(0, 101) <= 30)
                    CombatManager._instance.AddSubAction(new SpawnEnemyAction(BunnyWormEnemy.GetBunnyWorm(), Slots[i].SlotID, false, false, "Spawn_Basic"));

            yield break;
        }
    }
}

