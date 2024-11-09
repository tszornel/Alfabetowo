using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_Data_Tasks", menuName = "Webaby/Unit/Tasks Data", order = 1)]
public class TasksDataScriptableObject : ScriptableObject
{
    // Start is called before the first frame update

    [Header("Data")]
    public TasksDatabase tasksDatabase;
   
    [Header("Tasks")]
    [StringInList(typeof(PropertyDrawerHelper), "AllTasks")]
    public string[] friendTasks;
    public AbecadlowoTask[] abecadlowoTasksForFriend;


    private void Awake()
    {
        if (tasksDatabase == null)
            tasksDatabase = Resources.Load<TasksDatabase>("TasksDatabase");

        abecadlowoTasksForFriend = GetFriendTasks();
    }
    private AbecadlowoTask[] GetFriendTasks()
    {
        GameLog.LogMessage("GetFriendTasks entered");
        return tasksDatabase.GetTasksByName(friendTasks);

    }

    public AbecadlowoTask[] GetAllFriendTasks()
    {
        if (abecadlowoTasksForFriend.Length == 0) {
            abecadlowoTasksForFriend = GetFriendTasks();
            GameLog.LogMessage("abecadlowoTasksForFriend:"+abecadlowoTasksForFriend.Length);
        }
           
        return abecadlowoTasksForFriend;
    }

    





    AbecadlowoTask SelectNextTask(string[] tasksArray)
    {
        AbecadlowoTask localTask = null;
        if (tasksArray.Length <= 0)
        {
            return null;
        }
        for (int i = 0; i < tasksArray.Length; i++)
        {
            localTask = tasksDatabase.GetTaskByName(tasksArray[i]);
            if (localTask.done)
                continue;
            else
                break;


        }

        return localTask;


    }

    public AbecadlowoTask SelectNextTask()
    {
        return SelectNextTask(friendTasks);


    }
}


