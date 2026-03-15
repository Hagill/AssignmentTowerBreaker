using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private int currentStageNumber;
    private StageManager stageManager;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private GameObject monsterGroupGO;
    [SerializeField] private List<GameObject> bossMonsterPrefabs;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int bossStageNumber; // 몇 스테이지마다 출현할 것인지
    private MonsterGroup monsterGroup;

    private BossMonsterBineulI currentBossMonsterBineulI;
    private BossMonsterChiko currentBossMonsterChiko;

    public Transform PlayerSpawnPoint => playerSpawnPoint;

    private bool isEnter;
    private bool isBossStage;
    private bool isBossDie;

    public event Action OnStageClear;

    private void Awake()
    {
        isEnter = false;
    }

    void Start()
    {
        currentStageNumber = stageManager.CurrentStageNumber;

        if (currentStageNumber == 1)
        {
            isEnter = true;
            SpawnMonster();
        }
        else if(currentStageNumber % bossStageNumber == 0)
        {
            isBossStage = true;
        }
    }

    public void InitStageManager(StageManager stageManager)
    {
        this.stageManager = stageManager;
    }

    public void SpawnMonster()
    {
        GameObject monsterGroupInstance = Instantiate(monsterGroupGO, monsterSpawnPoint.position, Quaternion.identity);
        monsterGroupInstance.transform.SetParent(transform);
        monsterGroup = monsterGroupInstance.GetComponent<MonsterGroup>();
        monsterGroup.SetCurrentStageNumber(currentStageNumber);
        monsterGroup.OnAllMonsterDie += StageClear;
    }

    public void SpawnBossMonster()
    {
        int randomIndex = UnityEngine.Random.Range(0, bossMonsterPrefabs.Count);
        GameObject bossMonsterObject = EnemyObjectPoolManager.Instance.SpawnFromPool(bossMonsterPrefabs[randomIndex]);
        bossMonsterObject.transform.position = bossSpawnPoint.position;
        bossMonsterObject.transform.rotation = Quaternion.identity;
        bossMonsterObject.transform.SetParent(transform);

        Monster monsterComponent = bossMonsterObject.GetComponent<Monster>();

        if(monsterComponent != null)
        {
            monsterComponent.OnMonsterDied -= EventBossDie;
            monsterComponent.OnMonsterDied += EventBossDie;
            monsterComponent.ResetMonsterGroup(monsterGroup);
            monsterComponent.SetOriginalPrefab(bossMonsterPrefabs[randomIndex]);

            if (randomIndex == 0)
            {
                currentBossMonsterBineulI = monsterComponent.GetComponent<BossMonsterBineulI>();
            }
            else if (randomIndex == 1)
            {
                currentBossMonsterChiko = monsterComponent.GetComponent<BossMonsterChiko>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject playerGO = collision.gameObject;

        if (playerGO != null) 
        {
            if (((1 << playerGO.layer) & playerLayer) != 0)
            {
                if (!isEnter)
                {
                    isEnter = true;
                    GameManager.Instance.GameStartWithWaiting();
                    SpawnMonster();
                    if (isBossStage)
                    {
                        SpawnBossMonster();
                    }
                }
                else
                {
                    Player player = collision.GetComponent<Player>();
                    if (player != null)
                    {
                        player.TakeDamage(1);
                        monsterGroup.Knockback();
                        if (currentBossMonsterBineulI != null)
                        {
                            currentBossMonsterBineulI.Knockback();
                        }
                        else if (currentBossMonsterChiko != null)
                        {
                            currentBossMonsterChiko.Knockback();
                        }
                    }
                }
            }
        }
    }

    public void EventBossDie()
    {
        isBossDie = true;
        StageClear();
    }

    public void StageClear()
    {
        if (isBossStage)
        {
            if (!isBossDie || (monsterGroup != null && monsterGroup.Monsters.Count >0))
            {
                return;
            }
            Debug.Log("보스 스테이지 클리어");
            OnStageClear?.Invoke();
            return;
        }
        Debug.Log("스테이지 클리어");
        OnStageClear?.Invoke();
    }

    private void OnDisable()
    {
        monsterGroup.OnAllMonsterDie -= StageClear;
    }
}
