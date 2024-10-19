using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BunnyWorms.BunnyWormComponents
{
    public class BunnyWormVariationManager : MonoBehaviour
    {
        public SpriteRenderer BodyColor;
        public SpriteRenderer BunnyEars;
        public SpriteRenderer BunnyMouth;
        public SpriteRenderer BunnyEyes;
        public SpriteRenderer BunnyPattern;

        //public ParticleSystem ParticalRef;

        public void SetUp()
        {
            BodyColor = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            BunnyEars = BodyColor.transform.GetChild(0).GetComponent<SpriteRenderer>();
            BunnyMouth = BodyColor.transform.GetChild(1).GetComponent<SpriteRenderer>();
            BunnyEyes = BodyColor.transform.GetChild(2).GetComponent<SpriteRenderer>();
            BunnyPattern = BodyColor.transform.GetChild(3).GetComponent<SpriteRenderer>();
        }

        public void Awake()
        {
            Color32 Color1 = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
            Color32 Color2 = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);

            BunnyEars.sprite = MainClass.CopyColorData(MainClass.EarSprites.GetSprite(), Color1, Color2);
            BunnyMouth.sprite = MainClass.CopyColorData(MainClass.MouthSprites.GetSprite(), Color1, Color2);
            BunnyEyes.sprite = MainClass.CopyColorData(MainClass.EyeSprites.GetSprite(), Color1, Color2);

            Sprite PatternSprite = MainClass.BodyPatternSprites.GetSprite();
            BunnyPattern.sprite = PatternSprite == null? null : MainClass.CopyColorData(PatternSprite, Color1, Color2);

            BodyColor.sprite = MainClass.BodyColorSprites.GetSprite();

            //CopyGibs(transform.GetComponent<EnemyInFieldLayout_Data>(), Color1, Color2);
        }

        /*
        public void CopyGibs(EnemyInFieldLayout_Data DataRef, Color32 Color1, Color32 Color2)
        {

            Sprite[] GibsSprites = MainClass.CopyColorDataGibs(Color1, Color2);
            Sprite GibsSpriteThiongy = ResourceLoader.LoadSprite("Wormbunny_gibs");
            Sprite Test = MainClass.CopyColorData(ResourceLoader.LoadSprite("Wormbunny_gibs"), Color1, Color2, GibsSpriteThiongy.texture.width, GibsSpriteThiongy.texture.height, 0);

            ParticleSystem Partical = Instantiate(DataRef.m_Gibs, new Vector3(0, -25, 0), Quaternion.identity);
            DataRef.m_Gibs = Partical;
            ParticalRef = Partical;

            Debug.Log(Partical == null);
            Debug.Log(GibsSprites.Length);

            ParticleSystem GibsSystem = Partical.transform.GetChild(0).GetComponent<ParticleSystem>();
            var tsa = GibsSystem.textureSheetAnimation;
            tsa.enabled = false;
            tsa.enabled = true;
            tsa.mode = ParticleSystemAnimationMode.Sprites;
            tsa.numTilesX = 1;
            tsa.numTilesY = GibsSprites.Length;
            tsa.animation = ParticleSystemAnimationType.WholeSheet;
            for (int i = tsa.spriteCount - 1; i >= 0; i--)
            { tsa.SetSprite(i, GibsSprites[i]); }
            tsa.enabled = false;
            tsa.enabled = true;


            ParticleSystem GibsSystem2 = Partical.transform.GetChild(1).GetComponent<ParticleSystem>();
            var tsa2 = GibsSystem2.textureSheetAnimation;
            tsa2.enabled = false;
            tsa.enabled = true;
            tsa2.mode = ParticleSystemAnimationMode.Sprites;
            tsa2.numTilesX = 1;
            tsa2.numTilesY = GibsSprites.Length;
            tsa2.animation = ParticleSystemAnimationType.WholeSheet;
            for (int i = tsa2.spriteCount - 1; i >= 0; i--)
            { tsa2.SetSprite(i, GibsSprites[i]); }
            tsa2.enabled = false;
            tsa2.enabled = true;
        }

        public void OnDestroy()
        {
            if (ParticalRef != null)
                Destroy(ParticalRef.gameObject);
        }
        */
    }
}
