using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

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

    public void SetVFXVolume(float volume)
    {
        vfxSource.volume = volume;
    }

    public float GetVFXVolume()
    {
        return vfxSource.volume;
    }

    public void PlayVFX(AudioClip vfx) {
        vfxSource.clip = vfx;
        vfxSource.Play();
    }
}
