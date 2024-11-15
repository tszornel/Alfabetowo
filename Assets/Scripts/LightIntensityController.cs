using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightIntensityController : MonoBehaviour
{
    [SerializeField] float minIntensity = 0.0f;
    [SerializeField] float maxIntensity = 0.0f;
    [SerializeField] float minTime = 0.0f;
    [SerializeField] float maxTime = 0.0f;
   // [SerializeField] Light2D light;
    private IEnumerator Start()
    {
        Light2D light = GetComponent<Light2D>();
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
