using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Webaby.Utils;

#if UNITY_EDITOR
public class SetupLanguage:EditorWindow
{
    DialogLanguage language = DialogLanguage.Polski;
    private ItemObjectDatabase itemsDatabase = null;
    private ItemObjectDatabase lettersDatabase = null;

    [MenuItem("Webaby/SetupLanguage/SetupLanguage")]
    public static void ShowWindow()   
    {
        EditorWindow window;
      
        window = GetWindow<SetupLanguage>("Setup Language in the project");
        window.maxSize = new Vector2(315f, 310f);
        window.minSize = window.maxSize;
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate all scriptable language files in the project", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        language = (DialogLanguage)EditorGUILayout.EnumPopup("Choose Language:", language);
        EditorGUILayout.Space();
      
        itemsDatabase = (ItemObjectDatabase)EditorGUILayout.ObjectField("Items Database:", itemsDatabase, typeof(ItemObjectDatabase), true);
        lettersDatabase = (ItemObjectDatabase)EditorGUILayout.ObjectField("Letters Database:", lettersDatabase, typeof(ItemObjectDatabase), true);

        EditorGUI.BeginDisabledGroup(lettersDatabase == null && itemsDatabase == null);
   

        if (GUILayout.Button("Setup Items"))
        {
            SetLanguage(language, "items");
        }

        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("Setup Sprites Database"))
        {
            SetLanguage(language, "spriteDatabase");
        }

        if (GUILayout.Button("Setup Audio Database"))
        {
            SetLanguage(language, "audioDatabase");
        }

        if (GUILayout.Button("Setup Dialogs"))
        {
            SetLanguage(language, "dialogs");
        }

        if (GUILayout.Button("Setup Letters"))
        {
            SetLanguage(language, "letters");
        }
        if (GUILayout.Button("Setup Names"))
        {
            SetLanguage(language, "names");
        }
        if (GUILayout.Button("Setup Inventories"))
        {
            SetLanguage(language, "inventories");
        }
        if (GUILayout.Button("Setup Dropped Items"))
        {
            SetLanguage(language, "itemsToBeDropped");
          //  SetLanguage(language, "enemyItems");
          //  SetLanguage(language, "friendItems");
        }

        if (GUILayout.Button("Setup Items To Destroy"))
        {
            SetLanguage(language, "itemsToDestroy");
            //  SetLanguage(language, "enemyItems");
            //  SetLanguage(language, "friendItems");
        }

    }

    private void SetLanguage(DialogLanguage language, string type)
    {
        SetupItems.itemDatabase = itemsDatabase;
        SetupItems.letterDatabase = lettersDatabase;
       
        switch (type)
        {
            case "audioDatabase":
                AudioDatabase.FindAudio();
                break;
            case "spriteDatabase":
                SpritesDatabase.FindImages();
                break;
            case "dialogs":
                GameLog.LogMessage("language from setup language" + language.ToString());
                SetupDialogs.language = language;   
                SetupDialogs.CreateLanguageProcess(language);

                break;
            case "letters":
                SetupItems.language = language;     
                SetupItems.CreateLanguageFilesProcess(language, "Letters");
                break;
            case "names":
                SetupNames.language = language;     
                SetupNames.CreateLanguageFilesProcess(language);
                break;
            case "items":
                SetupItems.language = language;
                SetupItems.CreateLanguageFilesProcess(language,"Items");
                break;
            case "inventories":
                SetupInventories.language = language;
                SetupInventories.CreateLanguageFilesProcess(language);
                break;
            case "itemsToDestroy":
                SetupDropedItems.language = language;
                SetupDropedItems.CreateLanguageFilesProcess(language, "ItemsToDamageSO");
                break;
            case "itemsToBeDropped":
                SetupDropedItems2.language = language;
                SetupDropedItems2.CreateLanguageFilesProcess(language,"ItemsToBeDroppedSO");
                break;
            case "enemyItems":
                SetupDropedItems2.language = language;
                SetupDropedItems2.CreateLanguageFilesProcess(language,"EnemyItemsSO");
                break;
            case "friendItems":
                SetupDropedItems2.language = language;
                SetupDropedItems2.CreateLanguageFilesProcess(language,"FriendItemsSO");
                break;
            default:
                break;
        }
      
       
    }

   /* public static object STE(string[] @enum)
    {
        if (@enum.Length > 0)
        {
            XEnum xe = new XEnum("Enum");
            xe.addRange(@enum);
            return xe.getEnum();
        }
        else return null;
    }

    public static object STE(string sel, string[] @enum)
    {
        XEnum xe = new XEnum("Enum");
        xe.addRange(@enum);
        var obj = xe.getType();
        return Enum.Parse(obj, sel);
    }*/


}
#endif

