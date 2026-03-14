using UnityEngine;

public class Monster : Character
{
    private CharacterStateManager<Monster> monsterStateManager;
    [SerializeField] private Rigidbody2D monsterRb;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private Animator animator;
    [SerializeField] private float dieFlyTime;  // 사망시 날라가는 연출 시간
    [SerializeField] private float dieFlySpeed; // 사망시 날라가는 속도

    private MonsterGroup monsterGroup;

    public MonsterGroup MonsterGroup => monsterGroup;
    public CharacterStateManager<Monster> MonsterStateManager => monsterStateManager;
    public Rigidbody2D MonsterRb => monsterRb;
    public float DieFlyTime => dieFlyTime;
    public float DieFlySpeed => dieFlySpeed;
    public MonsterIdleState IdleState { get; private set; }
    public MonsterActiveState ActiveState { get; private set; }
    public MonsterDieState DieState { get; private set; }

    protected override void Awake()
    {
        monsterStateManager = new CharacterStateManager<Monster>(this);
        IdleState = new MonsterIdleState(this, monsterStateManager);
        ActiveState = new MonsterActiveState(this, monsterStateManager);
        DieState = new MonsterDieState(this, monsterStateManager);
    }

    protected override void Start()
    {
        base.Start();
        monsterGroup = GetComponentInParent<MonsterGroup>();
    }
    public override void Die()
    {
        monsterStateManager.ChangeState(DieState);
    }

    public override void TakeDamage(float damage)
    {
        float currentDamage = damage - monsterData.defencePoint;
        if (currentDamage <= 0) currentDamage = 0f;

        Hp -= currentDamage;
        if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }
    }
}
