using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float frequency = 25f;
    [SerializeField] private float amplitude = 0.3f;

    private Vector3 originalPosition;
    private float shakeTimer;
    private float shakeDuration;
    private bool isShaking = false;
    private float _frequency;
    private float _amplitude;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (isShaking)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                isShaking = false;
                transform.localPosition = originalPosition;
            }
            else
            {
                float progress = 1f - (shakeTimer / shakeDuration);
                float damping = Mathf.Cos(progress * Mathf.PI * 0.5f);
                
                float offsetX = Mathf.PerlinNoise(Time.time * _frequency, 0f) * 2f - 1f;
                float offsetY = Mathf.PerlinNoise(0f, Time.time * _frequency) * 2f - 1f;

                transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f) * _amplitude * damping;
            }
        }
    }

    public void Shake(float duration)
    {
        shakeTimer = duration;
        shakeDuration = duration;
        isShaking = true;
        _frequency = frequency;
        _amplitude = amplitude;
    }

    public void Shake(float duration, float frequency, float amplitude)
    {
        shakeTimer = duration;
        shakeDuration = duration;
        isShaking = true;
        _frequency = frequency;
        _amplitude = amplitude;
    }
}
