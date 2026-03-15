using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject stageGO;
    [SerializeField] private TextMeshProUGUI currentStageText;
    private Stage currentStage;
    private Vector3 lastStagePosition;
    [SerializeField] private float stageWidth;
    [SerializeField] private float playerMoveDuration;

    private int currentStageNumber;

    public int CurrentStageNumber => currentStageNumber;
    public Stage CurrentStage => currentStage;

    private void Awake()
    {
        currentStageNumber = 1;
    }

    private void Start()
    {
        CreateStage();
        InputStageText();
        SetCameraTarget(currentStage.transform);
    }

    public void InputStageText()
    {
        currentStageText.text = $"Stage: {currentStageNumber}";
    }

    public void CreateStage()
    {
        if (stageGO != null)
        {
            Vector3 newStagePosition;

            if (currentStageNumber == 1)
            {
                newStagePosition = Vector3.zero;
            }
            else
            {
                newStagePosition = lastStagePosition + Vector3.right * stageWidth;
            }

            GameObject stageInstace = Instantiate(stageGO, newStagePosition, Quaternion.identity);
            currentStage = stageInstace.GetComponent<Stage>();
            currentStage.OnStageClear += ListenStageClear;
            currentStage.InitStageManager(this);

            lastStagePosition = newStagePosition;
        }
    }

    private void SetCameraTarget(Transform target)
    {
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
    }

    private void ListenStageClear()
    {
        StartCoroutine(NextStage());
    }

    public IEnumerator NextStage()
    {
        currentStageNumber++;

        CreateStage();

        yield return MovePlayer(currentStage.PlayerSpawnPoint.position, playerMoveDuration);

        InputStageText();

        SetCameraTarget(currentStage.transform);
    }

    private IEnumerator MovePlayer(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = player.transform.position;
        float timer = 0f;

        while (timer < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPosition;
    }
}
