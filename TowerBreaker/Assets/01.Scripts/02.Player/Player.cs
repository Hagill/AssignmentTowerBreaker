using System;
using UnityEngine;
using static ConstValue;

public class Player : Character
{
    private GameManager gameManager;

    private CameraShaker cameraShaker;
    private CharacterStateManager<Player> playerStateManager;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerData playerData;
    // [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask monsterLayer;    // 몬스터 레이어
    [SerializeField] private Transform attackPosition;  // 공격 시작 위치
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakePower;

    public event Action OnHit;
    public event Action OnDie;

    public GameManager GameManager => gameManager;
    public PlayerData PlayerData => playerData;
    public Rigidbody2D PlayerRb => playerRb;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    public PlayerDieState DieState { get; private set; }

    protected override void Awake()
    {
        playerStateManager = new CharacterStateManager<Player>(this);
        IdleState = new PlayerIdleState(this, playerStateManager);
        HitState = new PlayerHitState(this, playerStateManager);
        DieState = new PlayerDieState(this, playerStateManager);

        if (playerData != null)
        {
            InitCharacterData(playerData.characterData);
        }
    }

    protected override void Start()
    {
        gameManager = GameManager.Instance;
        cameraShaker = Camera.main.GetComponent<CameraShaker>();
        playerStateManager.ChangeState(IdleState);
    }

    protected override void Update()
    {
        playerStateManager.Update();
    }

    protected override void FixedUpdate()
    {
        playerStateManager.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currentGameObject = collision.gameObject;

        if (((1<<currentGameObject.layer) & monsterLayer) != 0)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    public void OnAttack()
    {
        Vector2 direction = Vector2.right;

        RaycastHit2D[] hits = Physics2D.RaycastAll(attackPosition.position, direction, playerData.attackRange, monsterLayer);

        if (hits.Length > 0)
        {
            MonsterGroup group = hits[0].collider.GetComponent<MonsterGroup>();
            if (group != null && group.Monsters.Count > 0)
            {
                group.Monsters[0].TakeDamage(playerData.attackPoint);
            }
        }
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(AttackAnim);
    }

    public void Move()
    {
        animator.SetTrigger(MoveAnim);
        playerRb.AddForce(Vector2.right * playerData.characterData.moveSpeed, ForceMode2D.Impulse);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        playerStateManager.ChangeState(HitState);
        cameraShaker.ShakeCamera(shakePower, shakeDuration);
        OnHit?.Invoke();
    }
}
