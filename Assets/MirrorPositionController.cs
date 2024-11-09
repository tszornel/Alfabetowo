using UnityEngine;

public class MirrorPositionController : MonoBehaviour
{
    public GameObject objectToAlignWith;
   
    // Start is called before the first frame update
    void Start()
    {
        Vector3 worldPoint;
        if (objectToAlignWith) 
        {

            worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(objectToAlignWith.transform.position.x, 0, 0));
            worldPoint.x -= 0.8f;
            worldPoint.y = transform.position.y;
            worldPoint.z = transform.position.z;
            transform.position = worldPoint;
        
        }
        else 
        { 
            worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0,0));
            worldPoint.x -= GetComponent<RectTransform>().rect.width/2 +0.3f;
            worldPoint.y = transform.position.y;
            worldPoint.z = transform.position.z;
            transform.position = worldPoint;
        }

        
    }

 
   
}
