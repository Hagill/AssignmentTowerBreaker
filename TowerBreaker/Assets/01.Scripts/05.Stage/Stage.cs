using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private int currentStage;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private BoxCollider2D triggerPoint;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private GameObject monsterGroup;
    [SerializeField] private List<GameObject> bossMonsterPrefabs;
    [SerializeField] private LayerMask playerLayer;

    private bool isEnter;

    private void Awake()
    {
        isEnter = false;
    }

    void Start()
    {
        currentStage = stageManager.CurrentStage;
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

    void Update()
    {
        
    }

    public void SpawnMonster()
    {
        Instantiate(monsterGroup, monsterSpawnPoint.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject playerGO = collision.gameObject;

        if (((1 << playerGO.layer) & playerLayer) != 0)
        {
            if (!isEnter)
            {
                isEnter = true;
                SpawnMonster();
            }
            else
            {
                Player player = collision.GetComponent<Player>();
                if ( player != null)
                {
                    player.TakeDamage(1);
                }
            }
        }
    }
}
