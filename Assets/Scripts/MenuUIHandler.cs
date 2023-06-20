using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text bestPlayerScore;    
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private Button startNewGameBTN;

    private void Start()
    {
        ShowBestPlayerScore();        
    }

    private void FixedUpdate()
    {
        CheckPlayerNameIsPresent();
    }

    // Verifies if the player has entered a name in the input box
    // updates interactability of start new game button
    private void CheckPlayerNameIsPresent()
    {
        if (!playerName)
        {
            playerName = GameObject.Find("PlayerNameInputField").GetComponent<TMP_InputField>();
        }

        if (IsStartGameBtnPresent())
        {
            if (playerName.text.Length > 0)
            {
                startNewGameBTN.interactable = true;
            }
            else
            {
                startNewGameBTN.interactable = false;
            }
        }
    }

    // Verifies if there is a start new game button return true or false
    private bool IsStartGameBtnPresent()
    {
        if (!startNewGameBTN)
        {
            startNewGameBTN = GameObject.Find("StartBtn").GetComponent<Button>();
        }

        if (startNewGameBTN)
        {
            return true;
        }

        return false;
    }


    // Checks if there is a best score stored and shows the players name and score
    // also verifies that the text gameobjects are defined
    private void ShowBestPlayerScore()
    {
        if (!bestPlayerScore)
        {
            bestPlayerScore = GameObject.Find("BestPlayerScoreText").GetComponent<TMP_Text>();
        }        

        if (GameManager.instance.BestPlayer != null)
        {
            string bestScore = $"<color=green>{GameManager.instance.BestPlayer.name}</color> -> {GameManager.instance.BestPlayer.points} points";
            bestPlayerScore.text = bestScore;            
        }
        else
        {
            bestPlayerScore.text = "Best Score: <color=green>Can be you!</color>";            
        }
    }

    // Starts new game only if the player enters a name
    public void StartNewGame()
    {
        if (playerName)
        {
            string name = playerName.text;
            GameManager.instance.StartNewGame(name);
        }
    }

    // Finish the current game session
    public void ExitGame()
    {
        //TODO: Set a confirmation box here

        GameManager.instance.QuitGame();
    }
}
