using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float power, float duration)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCoroutine(power, duration));
    }

    private IEnumerator ShakeCoroutine(float power, float duration)
    {
        noise.m_AmplitudeGain = power;
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0f;
    }
}
