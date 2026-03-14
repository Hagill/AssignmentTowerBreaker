using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject stage;
    private int currentStage;

    public event Action changeStage;

    public int CurrentStage => currentStage;

    private void Awake()
    {
        currentStage = 1;
    }
}
