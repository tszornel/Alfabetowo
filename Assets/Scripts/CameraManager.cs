using Cinemachine;
using System.Collections;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager CinemaschineManagerInstance { get; private set; }
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startIntensity;
   // public CinemachineImpulseSource ShakeImpulseSource;
    void Awake()
    {
       CinemaschineManagerInstance = this;
       _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
       _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Shake() {
        GameLog.LogMessage("Shake Camera method entered");
        StartCoroutine(_ProcessShake(15, 1f));
    }
    private void ShakeCamera(float intensity, float timer)
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _shakeTimer = timer;
        _shakeTimerTotal = timer;
        _startIntensity = intensity;
    }
    private IEnumerator _ProcessShake(float shakeIntensity = 15f, float shakeTiming = 1f)
    {
        ShakeCamera(1, shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        ShakeCamera(0, 0);
    }
    /* private void Update()
     {
         if (_shakeTimer > 0)
         {
             _shakeTimer -= Time.deltaTime;
             _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
         }
     }*/
}
