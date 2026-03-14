using UnityEngine;

public class Monster : Character
{
    private CharacterStateManager<Monster> monsterStateManager;
    [SerializeField] private Rigidbody2D monsterRb;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private Animator animator;
    [SerializeField] private float dieFlyTime;  // 사망시 날라가는 연출 시간
    [SerializeField] private float dieFlySpeed; // 사망시 날라가는 속도
    
    private CameraShaker cameraShaker;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakePower;

    private MonsterGroup monsterGroup;

    public MonsterGroup MonsterGroup => monsterGroup;
    public CharacterStateManager<Monster> MonsterStateManager => monsterStateManager;
    public Rigidbody2D MonsterRb => monsterRb;
    public float DieFlyTime => dieFlyTime;
    public float DieFlySpeed => dieFlySpeed;
    public MonsterIdleState IdleState { get; private set; }
    public MonsterDieState DieState { get; private set; }

    protected override void Awake()
    {
        monsterStateManager = new CharacterStateManager<Monster>(this);
        IdleState = new MonsterIdleState(this, monsterStateManager);
        DieState = new MonsterDieState(this, monsterStateManager);

        if (monsterData != null)
        {
            InitCharacterData(monsterData.characterData);
        }
    }

    protected override void Start()
    {
        base.Start();
        cameraShaker = Camera.main.GetComponent<CameraShaker>();
        monsterGroup = GetComponentInParent<MonsterGroup>();
    }

    public override void Die()
    {
        monsterStateManager.ChangeState(DieState);
    }

    public override void TakeDamage(float damage)
    {
        float currentDamage = damage - monsterData.defencePoint;
        if (currentDamage <= 0) return;

        Hp -= currentDamage;
        if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }
        cameraShaker.ShakeCamera(shakePower, shakeDuration);
    }
}
