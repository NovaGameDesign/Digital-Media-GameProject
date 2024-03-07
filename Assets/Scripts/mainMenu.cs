using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DigitalMedia
{
    public class mainMenu : MonoBehaviour
    {
       public void PlayGame()
        {
            SceneManager.LoadScene("Main");
        }
       public void QuitGame()
        {
            Debug.Log("Quitting Game....");
            Application.Quit();
        }

    }
}
