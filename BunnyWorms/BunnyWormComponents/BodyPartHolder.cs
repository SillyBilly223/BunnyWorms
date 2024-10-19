using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BunnyWorms.BunnyWormComponents
{
    public class BodyPartHolder
    {
        public Sprite[] Sprites;

        public int ChanceThreshold;

        public int ChanceRange;

        public int FailReturn;

        public Dictionary<int, List<Sprite>> SavedColoredSprites = new Dictionary<int, List<Sprite>>();

        public Sprite GetSprite()
        {
            if (Sprites == null || Sprites.Length == 0)
            { Debug.LogError("Invalid BodyPartHolder"); return null; }

            int Amount = Random.Range(0, ChanceRange + 1);
            if (Amount <= ChanceThreshold)
                return Sprites[Random.Range(0, Sprites.Length)];
            if (FailReturn < Sprites.Length && FailReturn >= 0)
                return Sprites[FailReturn];

            return null;
        }

        public BodyPartHolder(Sprite[] sprites, int threshold = 100, int range = 100, int failReturn = -1)
        {
            Sprites = sprites;
            ChanceThreshold = threshold;
            ChanceRange = range;
            FailReturn = failReturn;
        }
    }
}
