using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;


    public void SetMusicVolume(float volume)
    {
        float logVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        masterMixer.SetFloat("MusicVolume", logVolume);
    }

    public void SetSFXVolume(float volume)
    {
        float logVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        masterMixer.SetFloat("SFXVolume", logVolume);
    }
}
