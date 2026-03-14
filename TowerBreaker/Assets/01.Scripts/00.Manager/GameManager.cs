using System;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    private bool isStart;
    public bool isPaused { get; private set; }
    public bool isWaiting { get; private set; }

    public event Action OnGameOver;

    public void GameStartWithWaiting()
    {
        isStart = true;
        isPaused = false;
        isWaiting = true;
        Time.timeScale = 1f;
    }

    public void GameActive()
    {
        isWaiting = false;
    }

    public void GameWaiting()
    {
        isWaiting = true;
    }

    public void GamePause()
    {
        isStart = false;
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void GameContinue()
    {
        isStart = true;
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
