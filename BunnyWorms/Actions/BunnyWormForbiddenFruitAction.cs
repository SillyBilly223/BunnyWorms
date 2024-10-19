using BunnyWorms.BunnyWormComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BunnyWorms.Actions
{
    public class BunnyWormForbiddenFruitAction : CombatAction
    {
        public static bool ActiveAction;

        public static void TryAddAction()
        {
            if (ActiveAction == true) return;
            CombatManager._instance.AddRootAction(new BunnyWormForbiddenFruitAction());
            ActiveAction = true;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            int CurrentBunnyWorms = 0;

            foreach (EnemyCombat Enemies in stats.EnemiesOnField.Values)
                if (Enemies.UnitTypes.Contains("BunnyWorm"))
                    CurrentBunnyWorms++;
            
            int BunnySpawnAmount = Mathf.FloorToInt(CurrentBunnyWorms / 2);

            for (int i = 0; i < BunnySpawnAmount; i++)
                CombatManager._instance.AddSubAction(new SpawnEnemyAction(BunnyWormEnemy.GetBunnyWorm(), -1, false, false, "Spawn_Basic"));

            ActiveAction = false;

            yield break;
        }
    }
}
