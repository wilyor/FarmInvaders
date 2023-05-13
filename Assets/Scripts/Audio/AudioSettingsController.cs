using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider audioSlider;

    /// <summary>
    /// Mueve los controladores a la posici�n que deber�an estar
    /// </summary>
    public void SetSliders()
    {
       musicSlider.value = PlayerPrefs.GetFloat("music");
       audioSlider.value = PlayerPrefs.GetFloat("audio");
    }

    public void SetSound(float volume)
    {
        PlayerPrefs.SetFloat("audio", volume);
        SoundManager.instance.SetSound(volume);
    }

    /// <summary>
    /// Cambia el volumen de la musica
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusic(float volume)
    {
        PlayerPrefs.SetFloat("music", volume);
        SoundManager.instance.SetMusic(volume);
    }
}
