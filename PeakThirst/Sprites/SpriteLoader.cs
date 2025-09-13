using System.IO;
using System.Reflection;
using UnityEngine;

internal static class SpriteLoader
{
    public static Sprite FromEmbedded(string resourceName, float pixelsPerUnit = 100f)
    {
        var asm = Assembly.GetExecutingAssembly();
        using (var stream = asm.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                Debug.LogError("[PeakThirst] Embedded resource not found: " + resourceName);
                return null;
            }

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                if (!ImageConversion.LoadImage(tex, ms.ToArray()))
                {
                    Debug.LogError("[PeakThirst] Failed to load image data for: " + resourceName);
                    return null;
                }
                tex.name = Path.GetFileNameWithoutExtension(resourceName);
                tex.filterMode = FilterMode.Bilinear;
                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                     new Vector2(0.5f, 0.5f), pixelsPerUnit);
            }
        }
    }
}
