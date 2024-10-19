using BepInEx;
using BrutalAPI;
using BunnyWorms.Actions;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Tools;
using UnityEngine;

namespace BunnyWorms
{
    public static class ExtraUtils
    {
        public static bool BunnyWormSpawnBypass;

        private static Hook ProcessSpecialSceneEndHook;
        private static ProcessSpecialSceneEndDelegate OriginalProcessSpecialSceneEnd;

        public static void InitializeCombat(Action<CombatManager> orig, CombatManager self)
        {
            //CombatStats
            orig(self);
            if (self._informationHolder._preCombatData.enemyBundle.Difficulty != BundleDifficulty.Boss || BunnyWormSpawnBypass)
                self.AddPriorityRootAction(new SpawnBunnyWormsAction());
        }

        public static void OnEmbarkPressed(Action<MainMenuController> orig, MainMenuController self)
        {
            BunnyWormAchivmentManager.ResetRunDeath();
            orig(self);
        }

        public static IEnumerator ProcessSpecialSceneEnd(object instance, string specialScene)
        {
            if (BunnyWormAchivmentManager.NoBunnyWormDeath()) { BunnyWormAchivmentManager.SaveNoDeath(); }
            IEnumerator originalRoutine = OriginalProcessSpecialSceneEnd(instance, specialScene);
            while (originalRoutine.MoveNext()) yield return originalRoutine.Current;
        }

        public static bool AddNewEnemy(Func<CombatStats, EnemySO, int, bool, string, bool> orig, CombatStats self, EnemySO enemy, int slot, bool givesExperience, string spawnSoundID)
        {
            if (enemy.unitTypes.Contains("BunnyWorm")) BunnyWormAchivmentManager.TickEncounterCount();
            return orig(self, enemy, slot, givesExperience, spawnSoundID);
        }

        public static void EnemyDeath(Action<EnemyCombat, DeathReference, string> orig, EnemyCombat self, DeathReference deathReference, string deathTypeID)
        {
            if (self.Enemy.unitTypes.Contains("BunnyWorm") && WasKilledByCharacter(deathReference)) BunnyWormAchivmentManager.TickDeathCount();
            orig(self, deathReference, deathTypeID);
        }

        public static bool WasKilledByCharacter(DeathReference deathReference)
        {
            return !deathReference.witheringDeath && deathReference.HasKiller && deathReference.killer.IsUnitCharacter;
        }

        /*
         * public void EnemyDeath(DeathReference deathReference, string deathTypeID)
        */

        private delegate IEnumerator ProcessSpecialSceneEndDelegate(object instance, string specialScene);

        public static void SetUp()
        {
            new Hook(typeof(CombatManager).GetMethod("InitializeCombat", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("InitializeCombat", (BindingFlags)(-1)));
            new Hook(typeof(CombatStats).GetMethod("AddNewEnemy", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("AddNewEnemy", (BindingFlags)(-1)));
            new Hook(typeof(EnemyCombat).GetMethod("EnemyDeath", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("EnemyDeath", (BindingFlags)(-1)));
            new Hook(typeof(MainMenuController).GetMethod("OnEmbarkPressed", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("OnEmbarkPressed", (BindingFlags)(-1)));

            ProcessSpecialSceneEndHook = new Hook(typeof(CombatManager).GetMethod("ProcessSpecialSceneEnd", (BindingFlags)(-1)), new ProcessSpecialSceneEndDelegate(ProcessSpecialSceneEnd));
            OriginalProcessSpecialSceneEnd = ProcessSpecialSceneEndHook.GenerateTrampoline<ProcessSpecialSceneEndDelegate>();
        }
    }
}
