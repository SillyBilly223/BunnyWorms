using BepInEx;
using BrutalAPI;
using BunnyWorms.BunnyWormComponents;
using MUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using Random = UnityEngine.Random;

namespace BunnyWorms
{
    [BepInPlugin("Whimsical.WackyBunnyWorm", "WackyBunnyWorm", "1.0.0")]
    [BepInDependency("BrutalOrchestra.BrutalAPI", BepInDependency.DependencyFlags.HardDependency)]
    public class MainClass : BaseUnityPlugin
    {
        public static BodyPartHolder BodyPatternSprites; //40%

        public static BodyPartHolder BodyColorSprites; //1/1000%

        public static BodyPartHolder EarSprites; //100%

        public static BodyPartHolder EyeSprites; //100%

        public static BodyPartHolder MouthSprites; //100%

        public static Sprite[] GibsRed;

        public static AssetBundle assetBundle;

        public void Awake()
        {
            assetBundle = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary("bunnyworm"));

            BunnyWormAchivmentManager.SetUp();

            ExtraUtils.SetUp();

            PrepareSprites();
            BunnyWormEnemy.Add();
        }

        public void Start()
        {
            BunnyWormAchivmentManager.AchievementCheckUp();
        }

        public void PrepareSprites()
        {
            
            GibsRed = CutOutSprites("Wormbunny_gibs", 0, 0, 32, 32);
            EarSprites = new BodyPartHolder(CutOutSprites("Wormbunny_Ears"));
            MouthSprites = new BodyPartHolder(CutOutSprites("Wormbunny_Mouth"));
            EyeSprites = new BodyPartHolder(CutOutSprites("Wormbunny_Eyes"));
            BodyPatternSprites = new BodyPartHolder(CutOutSprites("Wormbunny_BodyPattern"), 30);
            BodyColorSprites = new BodyPartHolder(CutOutSprites("Wormbunny_BodyColor", 2), 1, 1000, 0);
        }
        


        public Sprite[] CutOutSprites(string TextureRef, int WidthOffSet = 0, int HeightOffSet = 0, int SpriteWidth = 100, int SpriteHeight = 100, int PixlesPerUnit = 32)
        {
            Sprite Texture = ResourceLoader.LoadSprite(TextureRef);

            if (Texture == null ) return new Sprite[0];

            int Width = Mathf.FloorToInt(Texture.texture.width / SpriteWidth) - WidthOffSet;
            int Height = Mathf.FloorToInt(Texture.texture.height / SpriteHeight) - HeightOffSet;

            List<Sprite> Sprites =  new List<Sprite>();

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                { Sprites.Add(GetRandomCutOutSprite(Texture.texture, i * SpriteWidth, j * SpriteHeight, SpriteWidth, SpriteHeight, PixlesPerUnit)); Sprites[Sprites.Count - 1].name = $"{TextureRef}_{Sprites.Count - 1}"; }

            return Sprites.ToArray();
        }

        public Sprite GetRandomCutOutSprite(Texture2D Orgin, int XCord, int YCord, int Width, int Height, int ppu)
        {
            Color[] pixels = Orgin.GetPixels(XCord, YCord, Width, Height, 0);

            Texture2D newTexture = new Texture2D(Width, Height, TextureFormat.ARGB32, false) { anisoLevel = 1, filterMode = FilterMode.Point };
            newTexture.SetPixels(pixels);
            newTexture.Apply();

            return Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0f), ppu);
        }

        // 1 0 0 1
        // 0 1 0 1

        public static Color GreenColorRef => new Color(0f, 1f, 0f, 1f);
        public static Color RedColorRef => new Color(1f, 0f, 0f, 1f);
        
        public static Sprite CopyColorData(Sprite Sprite, Color32 Color1, Color32 Color2, int Width = 100, int Height = 100, float XPivot = 0.5f, float YPivot = 0)
        {
            Color[] pixels = Sprite.texture.GetPixels(0, 0, Width, Height, 0);

            Parallel.For(0, pixels.Length, i =>
            {
                if (pixels[i].Equals(GreenColorRef))
                    pixels[i] = Color1;
                else if (pixels[i].Equals(RedColorRef))
                    pixels[i] = Color2;
            });

            Texture2D newTexture = new Texture2D(Width, Height, TextureFormat.ARGB32, false) { anisoLevel = 1, filterMode = FilterMode.Point };
            newTexture.SetPixels(pixels);
            newTexture.Apply();

            return Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(XPivot, YPivot), 32f);
        }

        public static Sprite[] CopyColorDataGibs(Color32 Color1, Color32 Color2)
        {
            List<Sprite> GibsSprites = new List<Sprite>();

            string ColorID = GenerateColorID(Color1, Color2);

            for (int i = 0; i < GibsRed.Length; i++)
            { GibsSprites.Add(CopyColorData(GibsRed[i], Color1, Color2, GibsRed[i].texture.width, GibsRed[i].texture.height, 0, 0)); GibsSprites[GibsSprites.Count - 1].name = $"{GibsRed[i].name}_{ColorID}"; }

            return GibsSprites.ToArray();
        }

        public static string GenerateColorID(Color32 Color1, Color32 Color2)
        {
            return ((Color1.r + Color1.g + Color1.b + Color1.a) + (Color2.r + Color2.g + Color2.b + Color2.a)).ToString();
        }
    }
}
