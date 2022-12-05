using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading;

public class PauseMenu : SceneTransition
{
    [SerializeField] GameObject pauseMenu;

   public void Pause()
   {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
   }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        DataManager.WriteFile();
        SceneManager.LoadScene(sceneID);
    }
}
