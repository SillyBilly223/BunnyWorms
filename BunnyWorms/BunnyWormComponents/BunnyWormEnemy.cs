using BrutalAPI;
using BunnyWorms.Condition;
using BunnyWorms.CustomePassives;
using BunnyWorms.Effects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BunnyWorms.BunnyWormComponents
{
    public static class BunnyWormEnemy
    {
        public static EnemySO BunnyWormBase;

        public static Dictionary<int, EnemySO> CreatedBunnyWorms = new Dictionary<int, EnemySO>();

        public static void Add()
        {
            BunnyWormPassive ForbiddenBunnyWorm = ScriptableObject.CreateInstance<BunnyWormPassive>();
            ForbiddenBunnyWorm._passiveName = "Forbidden Fruit";
            ForbiddenBunnyWorm.m_PassiveID = "ForbiddenFruit_Bunny_ID";
            ForbiddenBunnyWorm.passiveIcon = LoadedAssetsHandler.GetPassive("ForbiddenFruit_InHerImage_PA").passiveIcon;
            ForbiddenBunnyWorm._enemyDescription = "At the end of the turn, foreach pair of BunnyWorms in combat. Spawn a random BunnyWorm.";
            ForbiddenBunnyWorm._characterDescription = "";
            ForbiddenBunnyWorm.doesPassiveTriggerInformationPanel = false;

            LoadedDBsHandler.PassiveDB.AddNewPassive(ForbiddenBunnyWorm.m_PassiveID, ForbiddenBunnyWorm);

            Enemy enemy = EXOP.EnemyInfoSetter("BunnyWorm", 3, Pigments.Red, EXOP._jumbleGutsClotted);
            enemy.enemy.name = "BunnyWorm_Base_EN";
            enemy.PrepareEnemyPrefab("Assets/WormBunnies/WormBunny.prefab", MainClass.assetBundle);
            enemy.enemy.enemyTemplate.gameObject.AddComponent<BunnyWormVariationManager>().SetUp();
            enemy.CombatSprite = ResourceLoader.LoadSprite("Wormbunny_icon.png");
            enemy.OverworldAliveSprite = ResourceLoader.LoadSprite("Wormbunny_icon.png", new Vector2?(new Vector2(0.5f, 0f)));
            enemy.OverworldDeadSprite = ResourceLoader.LoadSprite("Wormbunny_icon.png", new Vector2?(new Vector2(0.5f, 0f)));
            enemy.AddPassives(new BasePassiveAbilitySO[] { ForbiddenBunnyWorm, Passives.Overexert1, Passives.Withering });
            enemy.AddUnitType("BunnyWorm");
            enemy.AddLootData(null);

            #region ScriptableObject

            IndexEffectConditon IfFirstEffectTrue = ScriptableObject.CreateInstance<IndexEffectConditon>();
            IfFirstEffectTrue.EffectIndex = 0;
            IfFirstEffectTrue.wasSuccessful = true;

            IndexEffectConditon IfFirstEffectFalse = ScriptableObject.CreateInstance<IndexEffectConditon>();
            IfFirstEffectFalse.EffectIndex = 0;
            IfFirstEffectFalse.wasSuccessful = false;        

            #endregion ScriptableObject

            Ability ability = new Ability("Bunny", "Bunny_ID");
            ability.Description = "Spawn another random wormbunny.";
            ability.AbilitySprite = LoadedAssetsHandler.GetEnemy("Mung_EN").abilities[0].ability.abilitySprite;
            ability.Rarity.rarityValue = 55;
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<SpawnBunnyWormAnywhereEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot },
            };
            ability.Visuals = EXOP._splig.rankedData[0].rankAbilities[0].ability.visuals;
            ability.AnimationTarget = Targeting.Slot_SelfSlot;
            ability.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Other_Spawn.ToString() });

            Ability ability2 = new Ability("Worm", "Worm_ID");
            ability2.Description = "Either heals 1 health to the Opposing party member and absorbs 1 pigment of the Opposing party member's health color, or deals 1 damage to the Opposing party member.";
            ability2.AbilitySprite = LoadedAssetsHandler.GetEnemy("Mung_EN").abilities[0].ability.abilitySprite;
            ability2.Rarity.rarityValue = 55;
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ReturnRandomEffect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<HealEffect>(), entryVariable = 1, targets = Targeting.Slot_Front, condition = IfFirstEffectTrue },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ConsumeManaOfTargetHealth>(), entryVariable = 1, targets = Targeting.Slot_Front, condition = IfFirstEffectTrue },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 1, targets = Targeting.Slot_Front, condition = IfFirstEffectFalse }
            };
            ability2.Visuals = EXOP._splig.rankedData[0].rankAbilities[0].ability.visuals;
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { IntentType_GameIDs.Heal_1_4.ToString(), IntentType_GameIDs.Damage_1_2.ToString() });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Mana_Consume.ToString() });

            enemy.AddEnemyAbilities(new Ability[] { ability, ability2 });

            BunnyWormBase = enemy.enemy;
            enemy.AddEnemy();
        }

        public static int PreviousHealthID;

        public static EnemySO GetBunnyWorm()
        {
            int NameID = Random.Range(0, 2);
            int HealthID = Random.Range(0, 2000) == 1? 4 : Random.Range(0, 4);
            PreviousHealthID = HealthID;

            if (CreatedBunnyWorms.TryGetValue(NameID + HealthID, out EnemySO SavedBunnyVariation))
                return SavedBunnyVariation;

            string BunnnyName = NameID == 0 ? "BunnyWorm" : "WormBunny";

            SavedBunnyVariation = BunnyWormBase.Clone();
            SavedBunnyVariation.name = BunnnyName + "_" + NameID + HealthID + "_EN";
            SavedBunnyVariation._enemyName = BunnnyName;
            SavedBunnyVariation._locName = BunnnyName;
            SavedBunnyVariation.healthColor = GetManaByID(HealthID);


            LoadedDBsHandler.EnemyDB.AddNewEnemy(SavedBunnyVariation.name, SavedBunnyVariation);
            CreatedBunnyWorms.Add(NameID + HealthID, SavedBunnyVariation);
            return SavedBunnyVariation;
        }

        public static ManaColorSO GetManaByID(int ID)
        {
            switch(ID) 
            {
                case 0:
                    return Pigments.Red;
                case 1:
                    return Pigments.Yellow;
                case 2:
                    return Pigments.Blue;
                case 3:
                    return Pigments.Purple;
                case 4:
                    return Pigments.Green;
                default:
                    return Pigments.Red;
            }
        }
    }
}
