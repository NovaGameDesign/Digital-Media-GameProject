using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DigitalMedia
{
    public class PauseMenu : MonoBehaviour
    {
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
    }
}
