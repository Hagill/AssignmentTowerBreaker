using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private int currentStage;
    private StageManager stageManager;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private GameObject monsterGroupGO;
    [SerializeField] private List<GameObject> bossMonsterPrefabs;
    [SerializeField] private LayerMask playerLayer;

    private MonsterGroup monsterGroup;

    public Transform PlayerSpawnPoint => playerSpawnPoint;

    private bool isEnter;

    public event Action OnStageClear;

    private void Awake()
    {
        isEnter = false;
    }

    void Start()
    {
        currentStage = stageManager.CurrentStageNumber;

        if (currentStage == 1)
        {
            isEnter = true;
            SpawnMonster();
        }
        /*else if (currentStage % 10 == 0)
        {
            SpawnBossMonster();
        }*/
    }

    public void InitStageManager(StageManager stageManager)
    {
        this.stageManager = stageManager;
    }

    public void SpawnMonster()
    {
        GameObject monsterGroupInstance = Instantiate(monsterGroupGO, monsterSpawnPoint.position, Quaternion.identity);
        monsterGroup = monsterGroupInstance.GetComponent<MonsterGroup>();
        monsterGroup.OnAllMonsterDie += StageClear;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject playerGO = collision.gameObject;

        if (((1 << playerGO.layer) & playerLayer) != 0)
        {
            if (!isEnter)
            {
                isEnter = true;
                GameManager.Instance.GameStartWithWaiting();
                SpawnMonster();
            }
            else
            {
                Player player = collision.GetComponent<Player>();
                if ( player != null)
                {
                    player.TakeDamage(1);
                    monsterGroup.Knockback();
                }
            }
        }
    }

    public void StageClear()
    {
        Debug.Log("스테이지클리어");
        OnStageClear?.Invoke();
    }

    private void OnDisable()
    {
        monsterGroup.OnAllMonsterDie -= StageClear;
    }
}
