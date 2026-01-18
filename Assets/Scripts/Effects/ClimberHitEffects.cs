using UnityEngine;

public class ClimberHitEffects : MonoBehaviour
{
    [Header("Camera Shake on Damage")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeFrequencyScale = 75f;
    [SerializeField] private float shakeAmplitudeScale = 1f;

    [Header("Sprite Damage Flash Effect")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float damageFlashSpeed = 2f;
    [SerializeField] private float damageFlashDuration = 0.5f;


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

        DamageFlashEffect();
    }

    private void DamageFlashEffect()
    {
        if (spriteRenderer != null)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_DamageFlashSpeed", damageFlashSpeed);
            spriteRenderer.SetPropertyBlock(propBlock);
            CancelInvoke(nameof(ResetDamageFlash));
            Invoke(nameof(ResetDamageFlash), damageFlashDuration);
        }
    }

    private void ResetDamageFlash()
    {
        if (spriteRenderer != null)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_DamageFlashSpeed", 0f);
            spriteRenderer.SetPropertyBlock(propBlock);
        }
    }
}