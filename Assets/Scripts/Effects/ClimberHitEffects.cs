using UnityEngine;

public class ClimberHitEffects : MonoBehaviour
{
    [SerializeField] public float shakeDuration = 0.2f;
    [SerializeField] public float shakeFrequencyScale = 75f;
    [SerializeField] public float shakeAmplitudeScale = 1f;
    private CameraShake cameraShake;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    public void OnTakeDamage(float damage)
    {
        if (cameraShake != null)
        {
            cameraShake.Shake(shakeDuration, shakeFrequencyScale * damage, shakeAmplitudeScale * damage);
        }
    }
}
