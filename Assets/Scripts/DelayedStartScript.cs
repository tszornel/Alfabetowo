using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DelayedStartScript : MonoBehaviour
{
    public GameObject countDown;
    public GameObject player;
    public GameObject waveSpawner;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartDelayCoroutine");
    }
    IEnumerator StartDelayCoroutine() {
        Time.timeScale = 0;
        float pauseTime = Time.realtimeSinceStartup + 4f;
        while (Time.realtimeSinceStartup <= pauseTime)
            yield return 0;
        countDown.gameObject.SetActive(false);
        player.SetActive(true);
        GameLog.LogMessage("instantiate Wave spawner");
        waveSpawner.SetActive(true);
        Time.timeScale = 1;
    }
}
