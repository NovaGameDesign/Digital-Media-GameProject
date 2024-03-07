using DigitalMedia.Core;
using DigitalMedia.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

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
        
        private InputAction menu;
        
        //Pause Info
        public static bool GameIsPaused = false;
        public GameObject pauseMenuUI;
       
        void Start()
        
        {
            menu = _playerInput.actions["Menu"];
            menu.performed += Menu;
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
        public void MasterVolumeSlider()
        {
            float volume = masterSlider.value;
            audioMixer.SetFloat("master", Mathf.Log10(volume)*20);
        }
        public void MusicVolumeSlider()
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        }
        public void SFXVolumeSlider()
        {
            float volume = sfxSlider.value;
            audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
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
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
    }
}
