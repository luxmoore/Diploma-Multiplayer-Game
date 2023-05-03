using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIDirector : MonoBehaviour
{
    #region Variables
    [Header("Game Loop")]
    public GameObject helpMenu;
    public GameObject gameUI;
    public GameObject followerText;
    private bool helpSwitch = false;

    [Header("Lobby Screen")]
    public GameObject chooseMenu;
    public GameObject newMenu;
    public GameObject loadMenu;
    public TextMeshProUGUI playerOne;
    public TextMeshProUGUI playerTwo;
    public TextMeshProUGUI playerThree;
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

    #region Save System Buttons
    public void LoadLUXData()
    {

    }

    public void SaveLUXData()
    {

    }
    #endregion

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

    public void ChooseNewGame()
    {
        chooseMenu.SetActive(false);
        newMenu.SetActive(true);
    }

    public void ChooseLoadGame()
    {
        // check whether or not there is a loaded game. if so:

        chooseMenu.SetActive(false);
        loadMenu.SetActive(true);

        playerOne.SetText("");
        playerTwo.SetText("");
        playerThree.SetText("");

        // else grey out button and display text showing that there is no data to load
    }
}
