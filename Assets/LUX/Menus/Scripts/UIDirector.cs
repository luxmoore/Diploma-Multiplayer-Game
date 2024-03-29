using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIDirector : MonoBehaviour
{
    #region Variables
    [Header("Game Loop")]
    public GameObject helpMenu;
    public GameObject gameUI;
    public GameObject followerText;
    private bool helpSwitch = false;
    public SAVEMaster saveMaster;

    public GameObject hotfixText;
    public GameObject hotfixButtons;

    [Header("Lobby Screen")]
    public GameObject chooseMenu;
    public GameObject newMenu;

    public GameObject canLoadText;
    public GameObject loadButton;

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
        canLoadText.GetComponent<TextMeshProUGUI>().SetText("");
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
        saveMaster.ChangePlayerName(0, chosenName);
    }

    public void ChangeNamePlayerTwo(string chosenName)
    {
        Debug.Log("UIDirector : Player two's choice of name is " + chosenName);
        saveMaster.ChangePlayerName(1, chosenName);
    }

    public void ChangeNamePlayerThree(string chosenName)
    {
        Debug.Log("UIDirector : Player three's choice of name is " + chosenName);
        saveMaster.ChangePlayerName(2, chosenName);
    }
    #endregion

    public void ChooseLoadGame()
    {
        Debug.Log("Load game chosen.");

        // check whether or not there is a loaded game. if so:
        if(saveMaster.saveWasFound == true)
        {
            Debug.Log("Save file was found.");
            canLoadText.GetComponent<TextMeshProUGUI>().SetText("Save file was found.");

            chooseMenu.SetActive(false);
            loadMenu.SetActive(true);

            playerOne.SetText("Player 1, " + MetaStats.playerNames[0] + ", is at " + MetaStats.playerGridPosX[0] + ", " + MetaStats.playerGridPosY[0] + " with " + MetaStats.playerHealth[0] + "/100 health. Ratio is: " + MetaStats.playerDamageDealt[0] + "/" + MetaStats.playerHealthLost[0]);
            playerTwo.SetText("Player 2, " + MetaStats.playerNames[1] + ", is at " + MetaStats.playerGridPosX[1] + ", " + MetaStats.playerGridPosY[1] + " with " + MetaStats.playerHealth[1] + "/100 health. Ratio is: " + MetaStats.playerDamageDealt[1] + "/" + MetaStats.playerHealthLost[1]);
            playerThree.SetText("Player 3, " + MetaStats.playerNames[2] + ", is at " + MetaStats.playerGridPosX[2] + ", " + MetaStats.playerGridPosY[2] + " with " + MetaStats.playerHealth[2] + "/100 health. Ratio is: " + MetaStats.playerDamageDealt[2] + "/" + MetaStats.playerHealthLost[2]);
        }
        else
        {
            // grey out button and display text showing that there is no data to load
            Debug.Log("Save file was not found.");
            canLoadText.GetComponent<TextMeshProUGUI>().SetText("Save file not found.");

            loadButton.GetComponent<Button>().interactable = false;
            loadButton.GetComponent<Image>().color = Color.grey;
        }
    }

    public void SaveGame()
    {
        Debug.Log("Save button pressed");
        saveMaster.Save();
    }
}
