using UnityEngine;

public class BossMonsterBineulI : Monster
{
    [SerializeField] private float patternCooldown;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackDuration;

    private float currentCooldown;

    private bool isKnockback;

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
        currentCooldown -= Time.deltaTime;
        if (!GameManager.Instance.isWaiting)
        {
            if (!isKnockback)
            {
                Move();
            }
            if (currentCooldown <= 0)
            {

            }
        }
    }

    public void Move()
    {
        transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
    }

    public void bossPattern()
    {

    }
}
