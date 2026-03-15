using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterGroup : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] private List<GameObject> monsterPrefabs;  // 추가할 몬스터
    [SerializeField] private int monsterCount;  // 추가할 몬스터의 수
    [SerializeField] private float moveSpeed;
    [SerializeField] private float spawnXOffset;
    [SerializeField] private Transform groupTransform;
    [SerializeField] private BoxCollider2D groupCollider;
    [SerializeField] private float colliderY;
    [SerializeField] private float monsterHalfSizeX;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackDuration;

    private Coroutine coroutine;
    private List<Monster> monsters = new List<Monster>(); // 군집내 몬스터

    private bool isKnockback;

    public List<Monster> Monsters => monsters;
    public float MoveSpeed => moveSpeed;

    public event Action OnAllMonsterDie;

    private void Awake()
    {
        groupCollider = GetComponent<BoxCollider2D>();
        isKnockback = false;
    }

    void Start()
    {
        gameManager = GameManager.Instance;

        Vector3 startPosition = transform.position;

        for(int i = 0; i < monsterCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, monsterPrefabs.Count);
            Vector3 spawnPosition = new Vector3(startPosition.x + i * spawnXOffset, startPosition.y, startPosition.z);

            GameObject monsterObject = EnemyObjectPoolManager.Instance.SpawnFromPool(monsterPrefabs[randomIndex]);
            monsterObject.transform.position = spawnPosition;
            monsterObject.transform.rotation = Quaternion.identity;
            monsterObject.transform.SetParent(transform);

            Monster monsterComponent = monsterObject.GetComponent<Monster>();

            if (monsterComponent != null)
            {
                monsters.Add(monsterComponent);
                monsterComponent.OnMonsterDied -= CheckAllMonsterDie;
                monsterComponent.OnMonsterDied += CheckAllMonsterDie;
                monsterComponent.ResetMonsterGroup(this);
                monsterComponent.SetOriginalPrefab(monsterPrefabs[randomIndex]);
            }
        }

        ResizeCollider();
    }

    void Update()
    {
        if (!gameManager.isWaiting && !isKnockback)
        {
            MoveGroup();
        }
    }

    // 몬스터 군집의 Collider 조정
    public void ResizeCollider()
    {
        if (monsters.Count == 0)
        {
            groupCollider.size = Vector2.zero;
            return;
        }

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach(var monster in monsters)
        {
            Vector3 localPosition = monster.transform.localPosition;
            if (localPosition.x < minX) minX = localPosition.x - monsterHalfSizeX;
            if (localPosition.x > maxX) maxX = localPosition.x + monsterHalfSizeX;
        }

        float width = maxX - minX;

        groupCollider.size = new Vector2(width, colliderY);
        groupCollider.offset = new Vector2((maxX + minX) / 2f, 0);
    }

    public void MoveGroup()
    {
        groupTransform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    public void Knockback()
    {
        isKnockback = true;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        
        coroutine = StartCoroutine(KnockbackCoroutine(Vector2.right, knockbackDistance, knockbackDuration));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float distance, float duration)
    {
        Vector3 startPosition = groupTransform.position;
        Vector3 knockbackPosition = startPosition + direction.normalized * distance;
        float timer = 0f;

        while (timer < duration)
        {
            groupTransform.position = Vector3.Lerp(startPosition, knockbackPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        groupTransform.position = knockbackPosition;
        isKnockback = false;
    }

    public void CheckAllMonsterDie()
    {
        if (monsters.Count == 0)
        {
            OnAllMonsterDie?.Invoke();
        }
    }

    public void ChangeMoveSpeed(float speedValue)
    {
        moveSpeed = speedValue;
    }

    private void OnDisable()
    {
        OnAllMonsterDie = null;
    }
}
