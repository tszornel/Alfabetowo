
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.Universal;


public class LightsIntensityController : MonoBehaviour
{
    [SerializeField] float minIntensity = 0.0f;
    [SerializeField] float maxIntensity = 0.0f;
    [SerializeField] float minTime = 0.0f;
    [SerializeField] float maxTime = 0.0f;
    // [SerializeField] Light2D light;
    private IEnumerator Start()
    {
        Light2D[] lights = transform.GetComponentsInChildren<Light2D>();

        foreach (Light2D light in lights)
        {
            while (true)
            {
                float startIntensity = light.intensity;
                float targetIntensity = Random.Range(minIntensity, maxIntensity);
                float targetTime = Random.Range(minTime, maxTime);
                for (float t = 0; t < targetTime; t += Time.deltaTime)
                {
                    light.intensity = Mathf.Lerp(startIntensity, targetIntensity, t / targetTime);
                    yield return null;
                }
            }
        }
    }
}
