using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSlider.value = SoundManager.Instance.GetMusicVolume();
        sfxSlider.value = SoundManager.Instance.GetEffectVolume();
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSfxVolume);
    }

    void UpdateMusicVolume(float volume)
    {
        SoundManager.Instance.SetMusicVolume(volume);
    }

    void UpdateSfxVolume(float volume)
    {
        SoundManager.Instance.SetEffectVolume(volume);
    }


}
