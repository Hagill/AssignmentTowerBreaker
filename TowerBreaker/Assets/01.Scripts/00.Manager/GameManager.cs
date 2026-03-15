using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public bool isPaused { get; private set; }
    public bool isWaiting { get; private set; }
    public bool isStageClear { get; private set; }
    public bool isGameOver { get; private set; }

    public static Action<bool> OnSetMonstersCanMove;

    public static void SetMonstersMove(bool canMove)
    {
        OnSetMonstersCanMove?.Invoke(canMove);
    }

    public void GameStartWithWaiting()
    {
        isPaused = false;
        isWaiting = true;
        isStageClear = false;
        isGameOver = false;
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

    public void StageClear()
    {
        isStageClear = true;
    }

    public void GamePause()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void GameContinue()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void GameExit()
    {
        if (isGameOver)
        {
            isGameOver = false;
        }
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
    }
}
