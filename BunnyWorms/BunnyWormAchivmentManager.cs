using BepInEx;
using BrutalAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tools;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace BunnyWorms
{
    public static class BunnyWormAchivmentManager
    {
        public static readonly string Save_FolderName = "BunnyWormData";
        public static readonly string Save_FilePath = "BunnyWormData_Save";

        public static string Save_ModsPath => $"{Paths.GameRootPath}\\BrutalOrchestra_Data\\StreamingAssets\\mods\\{Save_FolderName}";
        public static string Save_FullPath => $"{Save_ModsPath}\\{Save_FilePath}";

        public static BunnyWormData CurrentData;

        public static void SetUp()
        {
            if (!GetSaveData()) Debug.LogError("Failed to Find/Create BunnyWorm_Wave");
            AddAchievements();
        }

        public static void AchievementCheckUp()
        {
            ValidateAchievements();
        }

        //LoadedDBsHandler.AchievementDB.

        public static bool GetSaveData()
        {
            if (!File.Exists(Save_FullPath) || !Directory.Exists(Save_ModsPath)) return GenerateSaveFile();
            else return LoadCurrentData();
        }

        public static bool GenerateSaveFile()
        {
            if (!Directory.Exists(Save_ModsPath)) Directory.CreateDirectory(Save_ModsPath);
            if (!File.Exists(Save_FullPath) && Directory.Exists(Save_ModsPath)) return SaveCurrentData(new BunnyWormData());
            return false;
        }

        public static bool SaveCurrentData(BunnyWormData NewData = null)
        {
            if (NewData != null) CurrentData = NewData;
            if (CurrentData == null) { Debug.LogError("Tryed to save null BunnyWormData"); return false; }
            string savedData = JsonUtility.ToJson(CurrentData, true);
            File.WriteAllText(Save_FullPath, savedData);
            return true;
        }

        public static bool LoadCurrentData()
        {
            if (!File.Exists(Save_FullPath) || !Directory.Exists(Save_ModsPath)) { return GenerateSaveFile(); }
            CurrentData = JsonUtility.FromJson<BunnyWormData>(File.ReadAllText(Save_FullPath));
            return true;
        }

        public static void AddAchievements()
        {
            AddBunnyWormAchievement(new ModdedAchievements("Xenogacha", "Encounter 100 Wormbunnies.", ResourceLoader.LoadSprite("WormBunny_Xenogacha"), "BunnyWorm_EncounterID"));
            AddBunnyWormAchievement(new ModdedAchievements("Exterminaat", "Kill 100 Bunnyworms.", ResourceLoader.LoadSprite("WormBunny_Exterminaat"), "BunnyWorm_KillID"));
            AddBunnyWormAchievement(new ModdedAchievements("Ecologically meaningless", "Don't kill a single Bunnyworm in an entire run.", ResourceLoader.LoadSprite("WormBunny_Meaningless"), "BunnyWorm_DontKillID"));
            AddBunnyWormAchievement(new ModdedAchievements("Creatures not made by God", "Encounter and/or kill 1000 Bunnyworms.", ResourceLoader.LoadSprite("WormBunny_Creature"), "BunnyWorm_CombineID"));
        }

        public static void ValidateAchievements()
        {
            if (CurrentData == null) { GenerateSaveFile(); return; }

            if (CurrentData.BunnyWorm_CombineData >= 1000) LoadedDBsHandler.AchievementDB._steamAchievements.TryUnlockModdedAchievement("BunnyWorm_CombineID");
            if (CurrentData.BunnyWorm_UnlockNoDeath) LoadedDBsHandler.AchievementDB._steamAchievements.TryUnlockModdedAchievement("BunnyWorm_DontKillID");

            if (CurrentData.BunnyWorm_DeathCount >= 100) LoadedDBsHandler.AchievementDB._steamAchievements.TryUnlockModdedAchievement("BunnyWorm_KillID");
            if (CurrentData.BunnyWorm_EncounterCount >= 100) LoadedDBsHandler.AchievementDB._steamAchievements.TryUnlockModdedAchievement("BunnyWorm_EncounterID");
        }

        public static void TryRemoveAchivementFromGameSave(string AchievementID)
        {
            if (LoadedDBsHandler.AchievementDB._steamAchievements._LoadedAchievements.moddedAchievementIDs.Contains(AchievementID))
                LoadedDBsHandler.AchievementDB._steamAchievements._LoadedAchievements.moddedAchievementIDs.Remove(AchievementID);
        }

        public static void AddBunnyWormAchievement(ModdedAchievements ach)
        {
            ach.AddNewAchievementToCUSTOMCategory("BunnyWorm_CG", "BunnyWorms");
        }

        #region Checks/Var
        public static void TickDeathCount()
        {
            if (!GetSaveData()) { NullSaveError(); return; }
            CurrentData.BunnyWorm_DeathCount++;
            CurrentData.BunnyWorm_RunDeath = true;
            SaveCurrentData();
            ValidateAchievements();
        }

        public static void TickEncounterCount()
        {
            if (!GetSaveData()) { NullSaveError(); return; }
            CurrentData.BunnyWorm_EncounterCount++;
            SaveCurrentData();
            ValidateAchievements();
        }

        public static void ResetRunDeath()
        {
            if (!GetSaveData()) { NullSaveError(); return; }
            CurrentData.BunnyWorm_RunDeath = false;
            SaveCurrentData();
        }

        public static bool NoBunnyWormDeath()
        {
            if (!GetSaveData()) { NullSaveError(); return false; }
            return !CurrentData.BunnyWorm_RunDeath;
        }

        public static void SaveNoDeath()
        {
            if (!GetSaveData()) { NullSaveError(); return; }
            CurrentData.BunnyWorm_UnlockNoDeath = true;
            SaveCurrentData();
            ValidateAchievements();
        }

        public static void NullSaveError()
        {
            Debug.LogError("Failed to Find/Create BunnyWorm_Wave");
        }

        #endregion Checks/Var
    }

    [System.Serializable]
    public class BunnyWormData
    {

        public bool BunnyWorm_ResetAchivments = false;

        public bool BunnyWorm_UnlockNoDeath = false;
        public bool BunnyWorm_RunDeath = false;
        public int BunnyWorm_DeathCount = 0;
        public int BunnyWorm_EncounterCount = 0;
        public int BunnyWorm_CombineData => BunnyWorm_EncounterCount + BunnyWorm_DeathCount;
    }
}
