using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class TestImageGenerator : MonoBehaviour
{
    private Material material;
    private Texture2D tex;

    private void Awake()
    {
        material = GetComponent<Renderer>().sharedMaterial;
        tex = material.mainTexture as Texture2D;
    }

    [Button]
    public void ResetTexture()
    {
        Material tmpMaterial = GetComponent<Renderer>().sharedMaterial;
        
        Texture2D tex = tmpMaterial.mainTexture as Texture2D;
        tex.filterMode = FilterMode.Point;

        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
                tex.SetPixel(i, j, Color.white);
        }

        tex.Apply();
    }

    public void SetPixel(Vector2 vector2, Color color, int radius)
    {
        Vector2Int pixelUV = new Vector2Int((int)(vector2.x * tex.width), (int)(vector2.y * tex.height));

        for (int y = pixelUV.y - radius; y <= pixelUV.y + radius; y++)
        {
            for (int x = pixelUV.x - radius; x <= pixelUV.x + radius; x++)
            {
                if ((x - pixelUV.x) * (x - pixelUV.x) + (y - pixelUV.y) * (y - pixelUV.y) > radius * radius)
                    continue;

                tex.SetPixel(x, y, color);
            }
        }

        tex.Apply(false);
    }

    //[Button]
    //public void GenerateTexture()
    //{
    //    Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
    //    tex.filterMode = FilterMode.Point;

    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //            tex.SetPixel(x, y, Color.white);
    //    }

    //    tex.Apply(false);

    //    for (int i = 0; i < uvPositions.Count; i++)
    //    {
    //        Vector2 pixelUV = uvPositions[i];
    //        pixelUV.x *= tex.width;
    //        pixelUV.y *= tex.height;

    //        Color pixelColor = tex.GetPixel(((int)pixelUV.x), ((int)pixelUV.y));
    //        tex.SetPixel(((int)pixelUV.x), ((int)pixelUV.y), pixelColor - new Color(0, 0.1f, 0.1f));
    //        tex.Apply(false);
    //    }

    //    test.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
    //        new Vector2(0.5f, 0.5f), pixelPerUnit);

    //    test.SetNativeSize();
    //}
}
