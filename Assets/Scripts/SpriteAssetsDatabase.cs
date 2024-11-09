using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "SpritesAtlasDatabase", menuName = "Webaby/SpritesAtlasDatabase")]
public class SpriteAssetsDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public SpriteAtlas[] spritesAssetsArray;


    public void OnBeforeSerialize()
    {

    }
    public void OnAfterDeserialize()
    {

    }

    public string[] GetSpriteObjectsNameArray()
    {

        List<string> stringNamesList = new List<string>();

        for (int i = 0; i < spritesAssetsArray.Length; i++)
        {
            stringNamesList.Add(spritesAssetsArray[i].name);
        }
        return stringNamesList.ToArray();

    }


    

    public SpriteAtlas GetSpriteAtlassObject(string name)
    {

        SpriteAtlas spriteObjects = null;

        for (int i = 0; i < spritesAssetsArray.Length; i++)
        {
            if (spritesAssetsArray[i].name.Equals(name))
            {
                spriteObjects = spritesAssetsArray[i];
                
            }
        }
        return spriteObjects;

    }


    public Sprite GetSpriteFromAtlass(string atlasName, string name)
    {

        Sprite sprite = null;

        for (int i = 0; i < spritesAssetsArray.Length; i++)
        {
            if (spritesAssetsArray[i].name.Equals(atlasName))
            {
                sprite = spritesAssetsArray[i].GetSprite(name);

            }
        }
        return sprite;

    }

}

