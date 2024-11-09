using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    public static CheckpointBehaviour[] CheckPointsList;
    public CheckpointData data;
    public Transform point;
    public Transform parentFrame;
    public CheckPointsInventory checkPointInventory;

    // Start is called before the first frame update


    private void Awake()
    {
        //if(CheckPointsList==null)
            CheckPointsList = GameObject.FindObjectsOfType<CheckpointBehaviour>(true);

        if (!checkPointInventory)
            checkPointInventory = GameObject.FindObjectOfType<CheckPointsInventory>(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameLog.LogMessage("CheckpointBehaviour On Trigger Enter2d entered");
        if (collision.tag == "Player")
        {
            DeactivateAllCheckPoints();
            Activate();
        }
    }

    public void SetCheckPointList(CheckpointBehaviour[] _CheckPointsList) 
    {
        CheckPointsList = _CheckPointsList;


    }

    public static void DeactivateAllCheckPoints()
    {

        GameLog.LogMessage("DeactivateAllCheckPoints entered CHeckPointList Lenght=" + CheckPointsList?.Length);
        if(CheckPointsList !=null && CheckPointsList.Length>0)
            foreach (var item in CheckPointsList)
            {
                GameLog.LogMessage("Deactivate " + item.name);
                item.Deactivate();
            }
    }

    public static void ResetAllCheckPoints()
    {

        GameLog.LogMessage("ResetAllCheckPoints entered CHeckPointList Lenght=" + CheckPointsList?.Length);
        if (CheckPointsList != null && CheckPointsList.Length > 0)
            foreach (var item in CheckPointsList)
            {
                GameLog.LogMessage("Reset " + item.name);
                item.data.Reset();
            }
    }

    public static void SetCheckPoint(string _checkPointName)
    {

        DeactivateAllCheckPoints();
            foreach (var item in CheckPointsList)
            {
                if(item.data.name == _checkPointName)
                    item.Activate();
            }



    }
    public void Deactivate()
    {
        data.active = false;
    }
    public void Activate()
    {
        if(checkPointInventory)
            checkPointInventory.Register(data);
        GameLog.LogMessage("Activate Checkpoint");
        data.active = true;
        data.registered = true;
    }
    public bool CheckActive() { return data.active; }

    public bool CheckRegistered() { return data.registered; }
    public static CheckpointBehaviour GetActiveCheckPoint()
    {
        if(CheckPointsList != null && CheckPointsList.Length>0)
            foreach (var item in CheckPointsList)
            {
                if (item.CheckActive())
                    return item;
            }
        return null;
    }
    public static Vector3 GetActiveCheckPointPosition()
    {
        Vector3 position = new Vector3(0, 0, 0);
        foreach (var item in CheckPointsList)
        {
            if (item.CheckActive())
            {
                if (item.point)
                    position = item.point.position;
                else
                    position = item.transform.position;
            }
        }
        return position;
    }

    public static void PrintCheckPoints() {

        if(CheckPointsList != null) 
            foreach (var item in CheckPointsList)
            {
                GameLog.LogMessage(item.name);
            }
    
    }

    private void OnApplicationQuit()
    {
        data.Deactivate();
    }

}
