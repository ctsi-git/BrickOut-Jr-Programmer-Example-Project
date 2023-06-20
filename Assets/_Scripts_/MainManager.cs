using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [Header("Bricks")]
    [SerializeField] private Brick brickPrefab;
    [SerializeField] private int lineCount = 6;
    [SerializeField] private Transform brickContainer;

    [Header("Ball")]
    [SerializeField] private Rigidbody ball;

    [Header("Visuals")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gamePausedPanel;

    const float step = 0.6f;

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        ShowBestScore();
        SetBricks();
        SetGameOverPanel();
        SetPausePanel();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartSession();
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartSession();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePauseGame();
            }
        }
    }

    // Check if the GameOver panel is defined
    private void SetGameOverPanel()
    {
        if (!gameOverPanel)
        {
            gameOverPanel = GameObject.Find("GameOverPanel");
        }
    }

    // Check if the pause panel is defined
    private void SetPausePanel()
    {
        if (!gamePausedPanel)
        {
            gamePausedPanel = GameObject.Find("PausePanel");
        }
    }

    // Pauses the game
    private void TogglePauseGame()
    {        
        if (gamePausedPanel.activeSelf)
        {
            gamePausedPanel.SetActive(false);
            Time.timeScale = 1; 
        }
        else
        {
            gamePausedPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Reload the current scene restarting the game
    private static void RestartSession()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Start the new session when SPACE key is press
    private void StartSession()
    {

        m_Started = true;
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        ball.transform.SetParent(null);
        ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);

    }


    // Set the bricks on the board
    private void SetBricks()
    {
        if (!brickContainer)
        {
            brickContainer = GameObject.Find("BrickContainer").transform;
        }

        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };

        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity, brickContainer);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    // Adds points to the score every time a block is destroy
    void AddPoint(int point)
    {
        m_Points += point;

        if (!scoreText)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        }

        scoreText.text = $"Score : {m_Points}";
    }

    // Shows the Best score player name and score
    private void ShowBestScore()
    {
        GameManager.instance.LoadBestPlayer();

        if (GameManager.instance.BestPlayer != null)
        {
            if (!bestScoreText)
            {
                bestScoreText = GameObject.Find("BestScoreText").GetComponent<TMP_Text>();
            }

            string bestScore = $"Best Score: <color=green>{GameManager.instance.BestPlayer.name}</color> ... {GameManager.instance.BestPlayer.score} points";
            bestScoreText.text = bestScore;
        }
    }


    // Handles the GameOver
    public void GameOver()
    {
        m_GameOver = true;
        gameOverPanel.SetActive(true);

        GameManager.instance.CurrentPlayerScore = m_Points;
        GameManager.instance.CheckPlayerScore();

    }
}
