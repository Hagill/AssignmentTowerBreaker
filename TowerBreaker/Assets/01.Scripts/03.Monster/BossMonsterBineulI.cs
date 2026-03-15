using System.Collections;
using UnityEngine;

public class BossMonsterBineulI : Monster
{
    [SerializeField] private float patternCooldown;
    [SerializeField] private float patternIncreaseValue;
    [SerializeField] private float patternDuration;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Transform itemDropPosition;
    [SerializeField] private GameObject itemPrefab;

    private float currentCooldown;

    private bool isKnockback;
    private Coroutine coroutine;
    private Coroutine patternCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentCooldown = patternCooldown;
    }

    protected override void Update()
    {
        if (!GameManager.Instance.isWaiting)
        {
            currentCooldown -= Time.deltaTime;
            
            if (!isKnockback)
            {
                Move();
            }
            if (currentCooldown <= 0)
            {
                currentCooldown = patternCooldown;
                if (patternCoroutine != null)
                {
                    StopCoroutine(patternCoroutine);
                }
                patternCoroutine = StartCoroutine(bossPattern());
            }
        }
    }

    public void Move()
    {
        transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
    }

    public IEnumerator bossPattern()
    {
        float prevMoveSpeed = MonsterGroup.MoveSpeed;
        MonsterGroup.ChangeMoveSpeed(patternIncreaseValue);
        yield return new WaitForSeconds(patternDuration);
        MonsterGroup.ChangeMoveSpeed(prevMoveSpeed);    // 기본 속도로 변경
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
        Vector3 startPosition = transform.position;
        Vector3 knockbackPosition = startPosition + direction.normalized * distance;
        float timer = 0f;

        while (timer < duration)
        {
            transform.position = Vector3.Lerp(startPosition, knockbackPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = knockbackPosition;
        isKnockback = false;
    }

    public override void Die()
    {
        float randomAttackPoint = Random.Range(1, 6);
        GameObject itemObject = Instantiate(itemPrefab, itemDropPosition.position, Quaternion.identity);
        itemObject.GetComponent<Item>().SetAttackPoint(randomAttackPoint);
        base.Die();
    }
}
