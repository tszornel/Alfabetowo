using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CreateAssetMenu(fileName = "SpritesDatabase", menuName = "Webaby/SpritesDatabase")]
public class SpritesDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public Sprite[] spritesArray;
    public static List<Sprite> imagesList;
    const string spriteDirectory = "Assets/Arts/Items/";
    const string spriteFriendsDirectory = "Assets/Arts/Friends/";
    public string savePath;

    public void OnBeforeSerialize()
    {
       // spritesArray = FindImages();
    }
    public void OnAfterDeserialize()
    {

    }

    public static void SearchDirectoryAsync(string targetDirectory)
    {

        string[] fileEntries = Directory.GetFiles(targetDirectory);

        foreach (string filePath in fileEntries)
        {
            if (!filePath.Contains("meta") && !filePath.Contains("Mask") && !filePath.Contains("Normal") && !filePath.Contains("mask") && !filePath.Contains("normal"))
            {
                string filePathNew = filePath.Replace(@"\", @"/");

                GameLog.LogMessage("sprite file:" + filePathNew);
                if (filePath.Contains("png") || filePath.Contains("psb") || filePath.Contains("jpg") || filePath.Contains("psd")) {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(filePathNew);
                   // if(sprite.GetType() == typeof(Sprite))  
                    imagesList.Add(sprite);


                }

                //  audioLenghtDict.Add(audioClip,(audioClip.length/100).ToString("f3"));
                char[] delimiterChars = { '.', '/', '\t' };
                string[] names = filePathNew.Split(delimiterChars);

            }
        }
        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            SearchDirectoryAsync(subdirectory);
        }
    }


    //public static void FindImagesStatic() => FindImages();

    [ContextMenu("FindImageFiles")]
    public Sprite[] GetNewSpriteArray() {

        spritesArray = FindImages();
        return spritesArray;
    }


   
    public static Sprite[] FindImages()
    {
        imagesList = new List<Sprite>();
        //  audioLenghtDict = new Dictionary<AudioClip, string>();
        // Process the list of files found in the directory.
        imagesList?.Clear();
        Object[] facesSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Arts/SpritesAtlases/names-atlas.png");
        Object[] iconSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Arts/SpritesAtlases/items-atlas.png");

        Object[] newFacesSprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Arts/Friends/HeadsForNames/firefly-heads-1.1.png");
        foreach (Object sprite in newFacesSprites)
        {
            GameLog.LogMessage("Sprite:" + sprite + "type=" + sprite.GetType());
            if (sprite.GetType() == typeof(Sprite))
                imagesList.Add((Sprite)sprite);

        }


        GameLog.LogMessage("faces count = " + facesSprites.Length);
        foreach (Object sprite in facesSprites)
        {
            GameLog.LogMessage("Sprite:" +sprite +"type="+ sprite.GetType());
            if(sprite.GetType() == typeof(Sprite))  
                imagesList.Add((Sprite)sprite);
          
        }

        foreach (Object sprite in iconSprites)
        {
            GameLog.LogMessage("Sprite:" + sprite + "type=" + sprite.GetType());
            if (sprite.GetType() == typeof(Sprite))
                imagesList.Add((Sprite)sprite);

        }

        SearchDirectoryAsync(spriteDirectory);
        SearchDirectoryAsync(spriteFriendsDirectory);
        Sprite[] spritesArray = imagesList.ToArray();

        return spritesArray;
        //GameLog.LogMessage("imagesList count po wyjsciu:" + imagesList.Count);

    }

    public string[] GetSpritesArray()
    {

        List<string> stringNamesList = new List<string>();

        for (int i = 0; i < spritesArray.Length; i++)
        {
            stringNamesList.Add(spritesArray[i].name);
        }
        return stringNamesList.ToArray();

    }


    public Sprite GetSpriteObject(string name)
    {

        Sprite spriteObject = null;

        for (int i = 0; i < spritesArray.Length; i++)
        {
            if (spritesArray[i].name.Equals(name))
            {
                spriteObject = spritesArray[i];
            }
        }
        return spriteObject;

    }


    [ContextMenu("Save")]
    public void Save()
    {
        //GameLog.LogMessage("Execute Save Inventory");
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat("C:/Users/tszor/Abecadlowo2022/Abecadlowo2021/Assets/", savePath));
        GameLog.LogMessage("Saved to C:/Users/tszor/Abecadlowo2022/Abecadlowo2021/Assets/ " + savePath);
        bf.Serialize(file, saveData);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        //GameLog.LogMessage("Execute Load Inventory");
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat("C:/Users/tszor/Abecadlowo2022/Abecadlowo2021/Assets/", spriteDirectory), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
        }

    }

}
#endif
