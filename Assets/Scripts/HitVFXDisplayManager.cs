using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVFXDisplayManager : MonoBehaviour
{

   // [Header("Object Pool of Hit VFX")]
   // public ObjectPoolBehaviour objectPool;

    public void ShowHitVFX(Transform hitTransform)
    {
        GameLog.LogMessage("Show Hit Fx");
        StartCoroutine(ShowHitVFXCouroutine(hitTransform));
        //hitVFXObject.transform.SetPositionAndRotation(hitTransform.position, hitTransform.rotation);
        
    }


    IEnumerator ShowHitVFXCouroutine(Transform hitTransform) {

        GameObject hitVFXObject = ObjectPoolerGeneric.Instance.SpawnFromPool("HitEffect", hitTransform.position, hitTransform.rotation, null);
        yield return new WaitForSeconds(1);
        ObjectPoolerGeneric.Instance.ReleaseToPool("HitEffect", hitVFXObject);
    }
}
