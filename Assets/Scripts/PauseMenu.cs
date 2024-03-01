using DigitalMedia.Core;
using DigitalMedia.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace DigitalMedia
{
    public class PauseMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;
        //Input Related
        [SerializeField] PlayerInput _playerInput;
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
        public void VolumeSlider(float volume)
        {
            Debug.Log(volume);
            audioMixer.SetFloat("volume",volume);
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
