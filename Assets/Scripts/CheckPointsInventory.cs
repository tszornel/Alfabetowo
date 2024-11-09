using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckPointsInventory : MonoBehaviour
{
    public List<GameObject> CheckPointsInventoryList = new List<GameObject>();
    public static List<CheckpointData> RegisteredCheckPointsList = new List<CheckpointData>();
    private GameObject activeCheckPoint;
    public List<CheckpointData> CheckPointsDataList;
    private CheckpointData activeCheckPointData;

    public static CheckPointsInventory Instance { get; private set; }

    public void Register(CheckpointData checkPoint)
    {
        GameLog.LogMessage("Register checkpoint entered" + checkPoint);
        var registered = RegisteredCheckPointsList.Any(checkPoint => checkPoint == true);

        if(!registered)
            RegisteredCheckPointsList.Add(checkPoint);
     
        GameLog.LogMessage("Register checkpoint left" + checkPoint);
    }

    public void UnRegister(CheckpointData checkPoint)
    {
        GameLog.LogMessage("Deregister checkpoint" + checkPoint);
        RegisteredCheckPointsList.Remove(checkPoint);
        //CheckPointsDataList.Remove(checkPoint.GetComponent<CheckpointBehaviour>().data);
    }

    private void Start()
    {
        if (CheckPointsDataList != null && CheckPointsDataList.Count > 0)
        {

            CheckpointData checkPoint = CheckPointsDataList[0];
            SetNewCheckPoint(checkPoint);
        }
    }
    private void SetNewCheckPoint(CheckpointData checkpoint)
    {
        if (checkpoint==null)
            return;

        GameLog.LogMessage("Setting new checkpoint" + checkpoint?.name);
        DeactivateAllCheckPoints();
        DeactivateAllCheckPointsData();
        foreach (GameObject item in CheckPointsInventoryList)
        {
            if (item.name.Equals(checkpoint.checkPointName))
            {
                SetCheckPointData(checkpoint);
                activeCheckPoint = item;
                activeCheckPoint.SetActive(true);
              
            }
        }
        
    }

    private void SetCheckPointData(CheckpointData item)
    {
        GameLog.LogMessage("Set checkPoints Data");
        activeCheckPointData = item;
        item.Activate();
 
    }

    private void DeactivateAllCheckPointsData() {

        foreach (var item in CheckPointsDataList)
        {
            item.Deactivate();
        }
    
    
    }

    private void DeactivateAllCheckPoints()
    {
        foreach (var item in CheckPointsInventoryList)
        {
            item.gameObject.SetActive(false);
        }
    }

    private CheckpointData GetNextCheckPoint(int index) {
        GameLog.LogMessage("GetNextCheckPoint entered index="+index+ " CheckPointsDataList.Count"+ CheckPointsDataList.Count);



        CheckpointData nextCheckPoint;
        if (index >= CheckPointsDataList.Count) 
        {
            index = 0;
            nextCheckPoint = CheckPointsDataList.ElementAtOrDefault(0);
        }
        else
            nextCheckPoint = CheckPointsDataList.ElementAtOrDefault(CheckPointsDataList.IndexOf(activeCheckPointData) + index);



        if (!nextCheckPoint)
           return CheckPointsDataList.ElementAtOrDefault(0);
        
        GameLog.LogMessage("Nextcheckpoint="+ nextCheckPoint +" registered="+ nextCheckPoint?.registered);

        if (nextCheckPoint.registered)
            return nextCheckPoint;
        else 
        {
            index = index + 1;
            GetNextCheckPoint(index);
        }

        return CheckPointsDataList.ElementAtOrDefault(0);

    }
    public void SetNextCheckPointFromList()
    {
        // CheckpointData nextCheckPoint = RegisteredCheckPointsList.ElementAtOrDefault(RegisteredCheckPointsList.IndexOf(activeCheckPointData) + 1);
        CheckpointData nextCheckPoint = GetNextCheckPoint(1);//CheckPointsDataList.ElementAtOrDefault(CheckPointsDataList.IndexOf(activeCheckPointData) + 1);

        /*if (nextCheckPoint == null)
            nextCheckPoint = RegisteredCheckPointsList.ElementAtOrDefault(0);*/
        
        SetNewCheckPoint(nextCheckPoint);
    }
    public void SetPreviousCheckPointFromList()
    {
        var previousCheckPoint = CheckPointsInventoryList.ElementAtOrDefault(CheckPointsInventoryList.IndexOf(activeCheckPoint) - 1);
        if (previousCheckPoint != null)
            SetNewCheckPoint(previousCheckPoint.GetComponent<CheckpointBehaviour>().data);
       
    }

}
