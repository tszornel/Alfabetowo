using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TasksManager : MonoBehaviour
{

    private TasksDatabase taskDatabase;
    List<AbecadlowoTask> activeTasks = new List<AbecadlowoTask>();
    public static TasksManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        taskDatabase = PickupItemAssets.Instance.tasksDatabase;

        if (!taskDatabase)
            taskDatabase = Resources.Load<TasksDatabase>("TasksDatabase");
    }

    public AbecadlowoTask GetTaskByName(string name)
    {
        return taskDatabase.GetTaskByName(name);


    }

    public AbecadlowoTask[] GetTasksByName(string[] names)
    {
        return taskDatabase.GetTasksByName(names);


    }

    public void AddToActiveTasks(string taskName)
    {

        activeTasks.Add(GetTaskByName(taskName));


    }

    public void AddToActiveTasks(AbecadlowoTask task)
    {

        activeTasks.Add(task);


    }

    public void RemoveFromActiveTasks(string taskName)
    {

        // var itemToRemove = activeTasks.Single(r => r.Id == GetTaskByName(taskName).Id);
        // activeTasks.Remove(itemToRemove);
        activeTasks.RemoveAt(GetTaskByName(taskName).Id);


    }

    public void RemoveFromActiveTasks(AbecadlowoTask task)
    {

        // var itemToRemove = activeTasks.Single(r => r.Id == GetTaskByName(taskName).Id);
        // activeTasks.Remove(itemToRemove);
        activeTasks.RemoveAt(task.Id);


    }


    public List<AbecadlowoTask> GetActiveTasksList()
    {
        return activeTasks;
    }

    public void ResetActiveTasks()
    {

        taskDatabase.ResetTasks();
    }


    public void StartTask(AbecadlowoTask _task) 
    {

        AddToActiveTasks(_task);
        _task.StartTask();


    }

    public void FinishTask(AbecadlowoTask _task)
    {

        RemoveFromActiveTasks(_task);
        _task.FinishTask();


    }

    private void OnApplicationQuit()
    {
        ResetActiveTasks();


    }

}







