using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
namespace DigitalMedia
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        //Input Related
        [SerializeField] PlayerInput _playerInput;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] public TMP_Dropdown ResDropDown;
        [SerializeField] public Toggle FullscreenToggle;
        Resolution[] AllResolutions;
        Resolution CurrentResolution;
        List<Resolution> SelectedResolutionList = new List<Resolution>();
        bool isFullscreen;
        int SelectedResolution;
        private InputAction menu;
        //Pause Info
        public static bool GameIsPaused = false;
        public GameObject pauseMenuUI;
       
        void Start()
        
        {
            menu = _playerInput.actions["Menu"];
            menu.performed += Menu;
            isFullscreen=true;
            AllResolutions=Screen.resolutions;
            List<string> ResolutionStringList = new List<string>();
            string newRes;
            foreach(Resolution res in AllResolutions)
            {
                newRes = res.width.ToString() + "x" + res.height.ToString();
                if (!ResolutionStringList.Contains(newRes)){
                    ResolutionStringList.Add(newRes);
                    SelectedResolutionList.Add(res);
                }
            }
            ResDropDown.AddOptions(ResolutionStringList);
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

        // Update is called once per frame
        void Menu (InputAction.CallbackContext context)
        {
            Debug.Log("Tried to pause");
            if(GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        public void ChangeResolution()
        {
            SelectedResolution =ResDropDown.value;
            CurrentResolution = SelectedResolutionList[SelectedResolution];
            Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, isFullscreen);
            PlayerPrefs.SetInt("ResW", CurrentResolution.width);
            PlayerPrefs.SetInt("ResH", CurrentResolution.height);
            
        }
        public void SetFullscreen()
        {
            isFullscreen = FullscreenToggle.isOn;

            Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, isFullscreen);
            PlayerPrefs.SetInt("Fullscreen", (isFullscreen ? 1 : 0));
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
        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
        void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }
        public void Quit()
        {
            Debug.Log("Quitting Game....");
            Application.Quit();
        }
        public void backtomain()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("StartScreen");
        }
        public void reloadScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main");
        }
       
        private void LoadSettings()
        {
            CurrentResolution = new Resolution();
            CurrentResolution.width= PlayerPrefs.GetInt("ResW", Screen.currentResolution.width);
            CurrentResolution.height= PlayerPrefs.GetInt("ResH", Screen.currentResolution.height);
            FullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) > 0;
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            Screen.SetResolution(
            CurrentResolution.width,
            CurrentResolution.height,
            FullscreenToggle.isOn
        );

            MasterVolumeSlider();
            MusicVolumeSlider();
            SFXVolumeSlider();
        }
    }
}
