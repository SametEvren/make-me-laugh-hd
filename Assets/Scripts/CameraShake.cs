using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    #region Singleton
    public static CameraShake Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    
    public float ShakeDuration = 0.5f; // Duration of the shake
    public float ShakeAmplitude = 1.2f; // Amplitude of the shake
    public float ShakeFrequency = 2.0f; // Frequency of the shake

    private float shakeElapsedTime = 0f;

    // Cinemachine FreeLook
    public CinemachineFreeLook FreeLookCam;

    // Cinemachine Noise Profile
    public CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        if (FreeLookCam != null)
            noise = FreeLookCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void TriggerShake()
    {
        shakeElapsedTime = ShakeDuration;
    }

    void Update()
    {
        if (shakeElapsedTime > 0)
        {
            // Set Cinemachine Camera Noise parameters
            noise.m_AmplitudeGain = ShakeAmplitude;
            noise.m_FrequencyGain = ShakeFrequency;

            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // Reset Cinemachine Camera Noise parameters
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}