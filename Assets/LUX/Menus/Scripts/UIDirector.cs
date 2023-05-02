using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDirector : MonoBehaviour
{
    #region Variables
    public GameObject helpMenu;
    public GameObject gameUI;
    public GameObject followerText;
    private bool helpSwitch = false;
    #endregion

    #region Boring Scene Thoroughfares
    public void BackToMain()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }

    public void Lobby()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneBuildIndex: 2);
    }

    public void KillProgram()
    {
        Application.Quit();
    }
    #endregion

    public void LoadLUXData()
    {

    }

    public void ToggleHelpScreen()
    {
        if(helpSwitch == false)
        {
            helpMenu.SetActive(true);
            followerText.SetActive(false);
            gameUI.SetActive(false);
            helpSwitch = true;
        }
        else
        {
            helpMenu.SetActive(false);
            followerText.SetActive(true);
            gameUI.SetActive(true);
            helpSwitch = false;
        }
    }
}
