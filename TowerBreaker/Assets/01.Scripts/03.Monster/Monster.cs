using System;
using UnityEngine;

public class Monster : Character
{
    protected CharacterStateManager<Monster> monsterStateManager;
    [SerializeField] protected Rigidbody2D monsterRb;
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float dieFlyTime;  // 사망시 날라가는 연출 시간
    [SerializeField] protected float dieFlySpeed; // 사망시 날라가는 속도

    protected CameraShaker cameraShaker;
    [SerializeField] protected float shakeDuration;
    [SerializeField] protected float shakePower;

    protected MonsterGroup monsterGroup;
    protected GameObject originalPrefab;

    public MonsterData MonsterData => monsterData;
    public MonsterGroup MonsterGroup => monsterGroup;
    public CharacterStateManager<Monster> MonsterStateManager => monsterStateManager;
    public Rigidbody2D MonsterRb => monsterRb;
    public Animator Animator => animator;
    public float DieFlyTime => dieFlyTime;
    public float DieFlySpeed => dieFlySpeed;
    public GameObject OriginalPrefab => originalPrefab;
    public MonsterIdleState IdleState { get; private set; }
    public MonsterDieState DieState { get; private set; }

    public event Action OnMonsterDied;

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

    protected virtual void OnEnable()
    {
        InitCharacterData(monsterData.characterData);
        monsterStateManager.ChangeState(IdleState);
    }

    protected override void Start()
    {
        base.Start();
        cameraShaker = Camera.main.GetComponent<CameraShaker>();
        monsterStateManager.ChangeState(IdleState);
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

    public void InvokeOnMonsterDied()
    {
        OnMonsterDied?.Invoke();
    }

    protected virtual void OnDisable()
    {
        monsterGroup = null;
        OnMonsterDied = null;
    }

    public void SetOriginalPrefab(GameObject original)
    {
        originalPrefab = original;
    }

    public void ResetMonsterGroup(MonsterGroup monsterGroup)
    {
        this.monsterGroup = monsterGroup;
    }
}
