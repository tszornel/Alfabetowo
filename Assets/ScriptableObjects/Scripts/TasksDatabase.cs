using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "TasksDatabase", menuName = "Task System/Database")]
public class TasksDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public AbecadlowoTask[] taskArray;
    // public DialogLanguage[] languages;
  
    public Dictionary<int, AbecadlowoTask> GetTaskByIDDictionary = new Dictionary<int, AbecadlowoTask>();
    private static List<AbecadlowoTask> tasksList;
    private static string tasksDirectory = "Assets/ScriptableObjects/TasksSO/SingleTasks";

    public void OnBeforeSerialize()
    {
        // GetDialogDictionary = new Dictionary<DialogLanguage, List<Dialog>>();
    }

    public void ResetTasks()
    {
        for (int i = 0; i < taskArray.Length; i++)
        {
            taskArray[i].ResetData();
        }


    }


#if UNITY_EDITOR
    [ContextMenu("FindTasksFiles")]
    public void GetTasks()
    {
        taskArray = FindTasks();
    }


    public static AbecadlowoTask[] FindTasks()
    {
        tasksList = new List<AbecadlowoTask>();
        //  audioLenghtDict = new Dictionary<AudioClip, string>();
        // Process the list of files found in the directory.
        tasksList?.Clear();
        SearchDirectoryAsync(tasksDirectory);
        AbecadlowoTask[] newTasksArray = tasksList.ToArray();
        return newTasksArray;

       // GameLog.LogMessage("audioClipList count po wyjsciu:" + tasksList.Count);

    }

    public static void SearchDirectoryAsync(string targetDirectory)
    {

        string[] fileEntries = Directory.GetFiles(targetDirectory);

        foreach (string filePath in fileEntries)
        {
            if (!filePath.Contains("meta"))
            {
                string filePathNew = filePath.Replace(@"\", @"/");

                GameLog.LogMessage("SO file:" + filePathNew);
                AbecadlowoTask taskSO = AssetDatabase.LoadAssetAtPath<AbecadlowoTask>(filePathNew);
                tasksList.Add(taskSO);

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
#endif


    public string[] GetTasksArray()
    {
        List<string> stringNamesList = new List<string>();
        for (int i = 0; i < taskArray.Length; i++)
        {
            stringNamesList.Add(taskArray[i].name);
        }
        return stringNamesList.ToArray();

    }
    
    public void OnAfterDeserialize()
    {

        for (int i = 0; i < taskArray.Length; i++)
        {
            if (taskArray[i]) {
                GetTaskByIDDictionary.Add(i, taskArray[i]);
                taskArray[i].Id = i;
            }
               

        }
        

    }

    public string[] GetNamesArray()
    {
        List<string> stringNamesList = new List<string>();
        for (int i = 0; i < taskArray.Length; i++)
        {
            stringNamesList.Add(taskArray[i].name);
        }
        return stringNamesList.ToArray();

    }



    public AbecadlowoTask GetTaskByName(string name)
    {

        for (int i = 0; i < taskArray.Length; i++)
        {
            if (taskArray[i].name == name)
                return taskArray[i];
        }
        GameLog.LogError("return null for name " + name);
        return null;


    }


    public AbecadlowoTask[] GetTasksByName(string[] names)
    {
       // GameLog.LogMessage("GetTasksByName lenght" + names.Length);
        List<AbecadlowoTask> TasksList = new List<AbecadlowoTask>();
        for (int i = 0; i < names.Length; i++)
        {
            for (int j = 0; j < taskArray.Length; j++)
            {
                if (taskArray[j].name == names[i])
                    TasksList.Add(taskArray[j]);    
            }
        }
        
       
        return TasksList.ToArray();


    }

    public void AddTaskToDatabase(AbecadlowoTask task)
    {


        GetTaskByIDDictionary.Add(task.Id, task);
    }


    public void ClearDatabase()
    {

        GetTaskByIDDictionary.Clear();
    }

}