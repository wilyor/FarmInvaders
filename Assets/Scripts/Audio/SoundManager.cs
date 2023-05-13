using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    #region VARIABLES
    public AudioMixer audioMixer;
    public static SoundManager instance;
    public float sounds;
    public float music;
    #endregion
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        SetSound(PlayerPrefs.GetFloat("audio"));
        SetMusic(PlayerPrefs.GetFloat("music"));
    }
    #region EVENTS
    #endregion

    #region METHODS
    /// <summary>
    /// Cambia el volumen de los efectos de audio
    /// </summary>
    /// <param name="volume"></param>
    public void SetSound(float volume)
    {
        PlayerPrefs.SetFloat("audio", volume);

        sounds = volume;
        audioMixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }

    /// <summary>
    /// Cambia el volumen de la musica
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusic(float volume)
    {
        PlayerPrefs.SetFloat("music", volume);
        Debug.Log(music);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    /// <summary>
    /// Cambia el volumen general
    /// </summary>
    /// <param name="volume"></param>
    public void SetMaster(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    #endregion
}
