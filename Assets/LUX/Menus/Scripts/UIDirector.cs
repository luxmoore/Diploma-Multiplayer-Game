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
    public SAVEMaster saveMaster;

    [Header("Game Set Up Screen")]
    public GameObject chooseMenu;
    public GameObject newMenu;
    public GameObject loadMenu;
    public TextMeshProUGUI playerOne;
    public TextMeshProUGUI playerTwo;
    public TextMeshProUGUI playerThree;

    public GameObject onlineMenu;
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
        MetaStats.isLoadedFromSave = false; // game will create a new save file in lobby, which just maintains player names and class.
                                            // Grid generation and player placement etc. will occur in Game Loop
        saveMaster.CreateNew();
    }

    private string input;
    public void ReadStringInput(string s)
    {
        input = s;
        Debug.Log(input);
    }

    #region Naming
    public void ChangeNamePlayerOne(string chosenName)
    {
        Debug.Log("UIDirector : Player one's choice of name is " + chosenName);
        saveMaster.ChangePlayerName(1, chosenName);
    }

    public void ChangeNamePlayerTwo(string chosenName)
    {
        Debug.Log("UIDirector : Player two's choice of name is " + chosenName);
        saveMaster.ChangePlayerName(2, chosenName);
    }

    public void ChangeNamePlayerThree(string chosenName)
    {
        Debug.Log("UIDirector : Player three's choice of name is " + chosenName);
        saveMaster.ChangePlayerName(3, chosenName);
    }
    #endregion

    public void BackToRoot()
    {
        chooseMenu.SetActive(true);
        onlineMenu.SetActive(false);
        loadMenu.SetActive(false);
    }

    public void ChooseLoadGame()
    {
        Debug.Log("Load game chosen.");

        // check whether or not there is a loaded game. if so:
        if(saveMaster.saveWasFound == true)
        {
            Debug.Log("Save file was found.");

            chooseMenu.SetActive(false);
            loadMenu.SetActive(true);

            playerOne.SetText("");
            playerTwo.SetText("");
            playerThree.SetText("");
        }
        else
        {
            Debug.Log("Save file was not found.");

            // grey out button and display text showing that there is no data to load

        }
    }

    public void ChooseOnline()
    {
        chooseMenu.SetActive(false);
        onlineMenu.SetActive(true);
    }
}
