using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BunnyWorms
{
    public static class ResourceLoader
    {
        public static Texture2D LoadTexture(string name, Assembly assembly = null)
        {
            if ((object)assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            byte[] array = ResourceBinary(name, assembly);
            if (array == null)
            {
                Debug.LogError("Missing Texture! Check for typos when using ResourceLoader.LoadSprite() and that all of your textures have their build action as Embedded Resource.");
                return null;
            }

            Texture2D texture2D = new Texture2D(0, 0, TextureFormat.ARGB32, mipChain: false)
            {
                anisoLevel = 1,
                filterMode = FilterMode.Point
            };
            texture2D.LoadImage(array);
            return texture2D;
        }

        public static Sprite LoadSprite(string name, Vector2? pivot = null, int ppu = 32, Assembly assembly = null)
        {
            if ((object)assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            Texture2D texture2D = LoadTexture(name, assembly);
            if (texture2D == null)
            {
                return null;
            }

            return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), pivot ?? new Vector2(0.5f, 0.5f), ppu);
        }

        public static byte[] ResourceBinary(string name, Assembly assembly = null)
        {
            if ((object)assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            string text = assembly.GetManifestResourceNames().FirstOrDefault((string x) => x == name || x.EndsWith("." + name) || x.Contains("." + name + "."));
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            using Stream stream = assembly.GetManifestResourceStream(text);
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }

        public static AssetBundle LoadAssetBundle(string bundlePath)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);
            if (assetBundle == null)
            {
                Debug.LogWarning($"Failed to load AssetBundle: {assetBundle}!");
                return null;
            }

            return assetBundle;
        }
    }
}
