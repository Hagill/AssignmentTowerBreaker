using System.Collections;
using UnityEngine;

public class BossMonsterChiko : Monster
{
    [SerializeField] private float IncreaseSpeedValue;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Transform itemDropPosition;
    [SerializeField] private GameObject itemPrefab;

    private bool isKnockback;
    private Coroutine coroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        if (!GameManager.Instance.isWaiting)
        {
            if (!isKnockback)
            {
                Move();
            }
        }
    }

    public void Move()
    {
        transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
    }

    public void IncreaseMoveSpeed()
    {
        MoveSpeed += IncreaseSpeedValue;
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