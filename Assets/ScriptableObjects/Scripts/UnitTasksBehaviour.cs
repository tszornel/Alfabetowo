using UnityEngine;

public class UnitTasksBehaviour : MonoBehaviour
{
    TasksManager manager;
    [Header("Tasks Data")]
    public TasksDataScriptableObject tasks;

    private static AbecadlowoTask currentTask;

    private void Awake()
    {
        manager = TasksManager.Instance;
        if (!tasks)
            GameLog.LogMessage("Brakuje ustawionych Taskow w " + transform.name);
    }

    public AbecadlowoTask SetCurrentTask(AbecadlowoTask _task) 
    { 
        currentTask = _task;
        TasksManager.Instance.StartTask(currentTask);
        return currentTask;
    }

    public void FinishCurrentTask() {

        TasksManager.Instance.FinishTask(currentTask);
        
    
    }

    public bool CheckAllTasksDone()
    {

        bool allTasksDone = true;

        GameLog.LogMessage("Tasks amount:"+ tasks?.GetAllFriendTasks().Length);
        foreach (var _task in tasks.GetAllFriendTasks())
        {
            GameLog.LogMessage("Task done ?"+_task.done);
            if (!_task.done)
            {
                GameLog.LogMessage("tasks");
                allTasksDone = false;
                break;
            }
        }
        return allTasksDone;
    }

    public AbecadlowoTask[] GetAllUnitTasks()
    {
        return tasks.GetAllFriendTasks();
    }


    public AbecadlowoTask GetNextUnitTask() {

        return tasks.SelectNextTask();
    }

    public AbecadlowoTask GetCurrentTask()
    {

        return currentTask;
    }



    public bool CheckPreviousTasksDone(AbecadlowoTask _task) 
    {
        bool previousTasksDone = true;
        AbecadlowoTask task;
        foreach (var item in _task.previousTasksNames)
        {
            task = TasksManager.Instance.GetTaskByName(item);
            if (task) 
            {
                previousTasksDone = task.CheckDone();
                if (previousTasksDone == false)
                    break;
            }
        }
         return previousTasksDone;
    }

}


