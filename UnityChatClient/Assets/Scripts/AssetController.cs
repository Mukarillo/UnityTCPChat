using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetController : MonoBehaviour
{
    private static Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
    private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        var sprites = Resources.LoadAll<Sprite>("Images/").ToList();

        foreach (Sprite sprite in sprites)
            icons.Add(sprite.name, sprite);

        var gameObjects = Resources.LoadAll<GameObject>("Prefabs/");

        foreach (var o in gameObjects)
            prefabs.Add(o.name, o);
    }

    public static GameObject GetGameObject(string name) => prefabs[name];
    public static Sprite GetSprite(string name) => icons[name];

    public static bool TryGetSprite(string name, out Sprite sprite)
    {
        sprite = null;
        if (icons.ContainsKey(name))
            sprite = icons[name];

        return icons.ContainsKey(name);
    }

    public static List<Sprite> GetAnimation(string prefix)
    {
        List<Sprite> sprites = new List<Sprite>();
        Sprite sprite;
        int index = 0;
        while (TryGetSprite(prefix + "_" + index.ToString("00"), out sprite))
        {
            sprites.Add(sprite);
            index++;
        }

        return sprites;
    }
}