using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : SceneTransition
{
    public void PlayGame ()
    {
        DataManager.ReadFile();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void QuitGame ()
    {
        DataManager.WriteFile();
        Application.Quit();
    }
}
