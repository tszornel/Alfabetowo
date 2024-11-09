using UnityEngine;


[CreateAssetMenu(fileName = "_Task", menuName = "Webaby/Unit/AbecadlowoTask")]

public class AbecadlowoTask : ScriptableObject
{
    public int Id;
    public string TaskName;
    public bool done;
    public bool active;
    public bool successPlayed;
    [TextArea]
    public string description;
    
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string relatedDialog;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string successDialog;
    [StringInList(typeof(PropertyDrawerHelper), "AllNames")]
    public string nameObjectName;
    public bool dependOnPreviousTasks;
    [StringInList(typeof(PropertyDrawerHelper), "AllTasks")]
    public string[] previousTasksNames;
    


    [StringInList(typeof(PropertyDrawerHelper), "AllItemsToBeDropped")]
    public string itemsTobedroppedName;

    private ItemsToBeDropped itemToBeDropped;

    public bool damaged;

    [Header("Interactable Object config")]
    [StringInList(typeof(PropertyDrawerHelper), "AllItems")]
    public string interactableItemName;
    public ItemsToBeDroppedDatabase itemsToBeDroppedDatabase;


    public ItemsToBeDropped GetItemsToBeDropped() {

        if (!itemToBeDropped) {

            if (!itemsToBeDroppedDatabase)
                itemsToBeDroppedDatabase = PickupItemAssets.Instance.itemsToBeDroppedDatabase;
            itemToBeDropped = itemsToBeDroppedDatabase.GetItemToBeDroppedByName(itemsTobedroppedName);


        }
           
        return itemToBeDropped;
    }


    public string[] GetSuccessItemNames() 
    {
       
        return itemToBeDropped.itemsToBeDropped;
        
    
    }

    public string GetSuccessDialogName()
    {
        return successDialog;
    }

    public string GetRelatedDialogName() 
    {
        return relatedDialog;
    }

    public void ResetData()
    {
        bool condition = TaskName.Equals("Empty Task");
        done = condition ? true : false;
        successPlayed = condition ? true : false;   
        active = false;
    }



    public bool CheckDone()
    {
        return done;

    }

    public bool CheckActive()
    {
        return done;

    }

    public void StartTask()
    {
        GameLog.LogMessage("Task" + this.name + " started");
        active = true;

    }

    public void FinishTask() 
    {
        GameLog.LogMessage("FinishTask entered:"+ TaskName);
        done = true;
        active = false;
        successPlayed = true;

    }

    public bool CheckPreviousTasksDone()
    {
        bool previousTasksDone = true;
        AbecadlowoTask task;
        foreach (var item in this.previousTasksNames)
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

    public bool isActive() {


        return active;
    }



}

