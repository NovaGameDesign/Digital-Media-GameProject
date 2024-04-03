using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
namespace DigitalMedia
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        void Start()
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                LoadSettings();
            }
            else
            {
                MasterVolumeSlider();
                MusicVolumeSlider();
                SFXVolumeSlider();
            }
        }
        public void MasterVolumeSlider()
        {
            float volume = masterSlider.value;
            audioMixer.SetFloat("master", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("masterVolume", volume);
        }
        public void MusicVolumeSlider()
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }
        public void SFXVolumeSlider()
        {
            float volume = sfxSlider.value;
            audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("sfxVolume", volume);
        }
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
        private void LoadSettings()
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            MasterVolumeSlider();
            MusicVolumeSlider();
            SFXVolumeSlider();
        }
    }
}
