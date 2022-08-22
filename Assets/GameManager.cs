using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    Score newScore;
    void Start()
    {
        Time.timeScale = 1;
        newScore = FindObjectOfType<Score>();
    }
    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        newScore.UpdatePoints();
        Time.timeScale = 0;
    }
    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }
}