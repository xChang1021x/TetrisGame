using System;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject player1LosePanel;
    public GameObject player2LosePanel;
    public Board2P boardplayer1;
    public Board2P boardplayer2;
    public Text timeText;
    public bool isPaused { get; set; }
    public bool isGameOver { get; set; }
    public bool isGameOver1 { get; set; }
    public bool isGameOver2 { get; set; }
    public float time { get; set; }

    void Start()
    {
        isPaused = false;
        isGameOver = false;
        isGameOver1 = false;
        isGameOver2 = false;
        time = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (isPaused || isGameOver)
        {
            return;
        }

        time += Time.deltaTime;

        // 将时间转换为 TimeSpan（方便格式化）
        System.TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        // 格式化为 MM:SS.ff（分钟:秒.毫秒）
        timeText.text = string.Format("{0:D2}:{1:D2}",
            timeSpan.Minutes,
            timeSpan.Seconds);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        boardplayer1.isPaused = isPaused;
        boardplayer2.isPaused = isPaused;
        if (isPaused)
        {
            Transform scoreObject = pausePanel.transform.Find("Score");
            Text scoreText = scoreObject.GetComponent<Text>();
            scoreText.text = boardplayer1.score.ToString();
            Transform scoreObject2 = pausePanel.transform.Find("Score2");
            Text scoreText2 = scoreObject2.GetComponent<Text>();
            scoreText2.text = boardplayer2.score.ToString();
            Transform timeObject = pausePanel.transform.Find("Time");
            Text timeText = timeObject.GetComponent<Text>();
            System.TimeSpan timeSpan = TimeSpan.FromSeconds(this.time);
            timeText.text = string.Format("{0:D2}:{1:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds);
            SoundManager.Instance.PlayPauseSound();
        }
        else
        {
            SoundManager.Instance.PlayResumeSound();
        }
    }

    public void GameOver(int playerId)
    {
        if (playerId == 1)
        {
            isGameOver1 = true;
            player1LosePanel.SetActive(true);
        }
        else
        {
            isGameOver2 = true;
            player2LosePanel.SetActive(true);
        }

        if (isGameOver1 && isGameOver2)
        {
            isGameOver = true;
            gameOverPanel.SetActive(true);
            Transform titleObject = gameOverPanel.transform.Find("Title");
            Text titleText = titleObject.GetComponent<Text>();
            if (boardplayer1.score > boardplayer2.score)
            {
                titleText.text = "Player 1 Wins!";
            }
            else if (boardplayer1.score < boardplayer2.score)
            {
                titleText.text = "Player 2 Wins!";
            }
            else
            {
                titleText.text = "Draw!";
            }


            Transform scoreObject = gameOverPanel.transform.Find("Score");
            Text scoreText = scoreObject.GetComponent<Text>();
            scoreText.text = boardplayer1.score.ToString();
            Transform scoreObject2 = gameOverPanel.transform.Find("Score2");
            Text scoreText2 = scoreObject2.GetComponent<Text>();
            scoreText2.text = boardplayer2.score.ToString();
            Transform timeObject = gameOverPanel.transform.Find("Time");
            Text timeText = timeObject.GetComponent<Text>();
            System.TimeSpan timeSpan = TimeSpan.FromSeconds(this.time);
            timeText.text = string.Format("{0:D2}:{1:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds);

        }
    }

    public void ResetGame()
    {
        boardplayer1.ResetGame();
        boardplayer2.ResetGame();
        isGameOver = false;
        isGameOver1 = false;
        isGameOver2 = false;
        gameOverPanel.SetActive(false);
        player1LosePanel.SetActive(false);
        player2LosePanel.SetActive(false);
        time = 0f;
        timeText.text = "00:00";
    }

    public void PlayButtonSound()
    {
        SoundManager.Instance.PlayButtonSound();
    }
}
