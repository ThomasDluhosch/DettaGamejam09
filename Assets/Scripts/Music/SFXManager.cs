using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [SerializeField] private AudioSource vfxSource;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSFXVolume(float volume)
    {
        vfxSource.volume = volume;
    }

    public float GetSFXVolume()
    {
        return vfxSource.volume;
    }

    public void PlaySFX(AudioClip vfx)
    {
        vfxSource.clip = vfx;
        vfxSource.Play();
    }
}
