using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
    
    public List<Monster> Monsters => monsters;

    private void Awake()
    {
        groupCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
    }

    void Start()
    {
        gameManager = GameManager.Instance;

        Vector3 startPosition = transform.position;

        for(int i = 0; i < monsterCount; i++)
        {
            int randomIndex = Random.Range(0, monsterPrefabs.Count);
            Vector3 spawnPosition = new Vector3(startPosition.x + i * spawnXOffset, startPosition.y, startPosition.z);

            GameObject monsterObject = Instantiate(monsterPrefabs[randomIndex], spawnPosition, Quaternion.identity, transform);
            Monster monsterComponent = monsterObject.GetComponent<Monster>();

            if (monsterComponent != null)
            {
                monsters.Add(monsterComponent);
            }
        }

        ResizeCollider();
    }

    void Update()
    {
        if (gameManager.isWaiting == false)
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
    }
}
