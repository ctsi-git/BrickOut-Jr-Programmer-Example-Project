using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private BestPlayer bestPlayer;

    private string currentPlayerName;
    private int currentPlayerScore;

    public BestPlayer BestPlayer { get => bestPlayer; set => bestPlayer = value; }
    public int CurrentPlayerScore { get => currentPlayerScore; set => currentPlayerScore = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadBestPlayer();

    }

    // Starts a New game
    public void StartNewGame(string playerName)
    {
        currentPlayerName = playerName;

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Handles the Game over
    public void CheckPlayerScore()
    {
        if(bestPlayer == null || currentPlayerScore > bestPlayer.score)
        {
            SaveBestPlayer();        
        }
    }

    // Saves the new best player data to disk
    public void SaveBestPlayer()
    {
        BestPlayer data = new BestPlayer();
        data.name = currentPlayerName;
        data.score = currentPlayerScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    
    // Loads from disk the best player data
    public void LoadBestPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BestPlayer data = JsonUtility.FromJson<BestPlayer>(json);

            bestPlayer = data;
        }
        else
        {
            bestPlayer = null;
        }
    }



    // Ends the current game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }


    
}


[System.Serializable]
public class BestPlayer
{
    public string name;
    public int score;
}