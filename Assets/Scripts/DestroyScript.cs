using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DestroyScript : MonoBehaviour {
    public float destroyTime;
	public bool pooledObject;

	public bool destroyEffect;
	// Use this for initialization
	void Start () {
		if (!pooledObject)
			Destroy(gameObject, destroyTime);
		else
		{
			StartCoroutine(DeactivateObjectCoroutine());
		}
	}
    private IEnumerator DeactivateObjectCoroutine() 
	{
		yield return new WaitForSeconds(destroyTime);
		IPooled objectForRelease = gameObject.GetComponent<IPooled>();
		ReturnToPool(objectForRelease, true);

	}



	void ReturnToPool(IPooled objectToRelease, bool effect)
    {

		objectToRelease.ReleseToPool();
		if (destroyEffect && effect)
			ObjectPoolerGeneric.Instance.SpawnFromPool("DestroyEffects", transform.position, Quaternion.identity, null);

	}
}
