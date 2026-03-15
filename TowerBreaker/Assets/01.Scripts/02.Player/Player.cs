using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static ConstValue;

public class Player : Character
{
    private GameManager gameManager;
    private CameraShaker cameraShaker;
    private CharacterStateManager<Player> playerStateManager;
    [SerializeField] private GameSceneManager gameSceneManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerData playerData;
    // [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask monsterLayer;    // 몬스터 레이어
    [SerializeField] private Transform attackPosition;  // 공격 시작 위치
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakePower;
    [SerializeField] private float knockbackDuration;

    private bool isKnockback;

    private bool isHit;

    public event Action OnDie;

    public GameSceneManager GameSceneManager => gameSceneManager;
    public GameManager GameManager => gameManager;
    public PlayerData PlayerData => playerData;
    public Rigidbody2D PlayerRb => playerRb;
    public bool IsHit => isHit;
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

        isKnockback = false;
        isHit = false;
    }

    protected override void Start()
    {
        gameManager = GameManager.Instance;
        cameraShaker = Camera.main.GetComponent<CameraShaker>();
        playerStateManager.ChangeState(IdleState);
        gameSceneManager.ChangeHp(Hp);
    }

    private void OnDisable()
    {
        OnDie = null;
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

    public void Defence()
    {
        Vector2 direction = Vector2.right;

        RaycastHit2D[] hits = Physics2D.RaycastAll(attackPosition.position, direction, playerData.defenceRange, monsterLayer);

        if (hits.Length > 0)
        {
            var hit = hits[0];

            MonsterGroup group = hit.collider.GetComponent<MonsterGroup>();
            if (group != null)
            {
                group.Knockback();
                PlayerKnockback();
                return;
            }

            BossMonsterBineulI bineulI = hit.collider.GetComponent<BossMonsterBineulI>();
            if (bineulI != null)
            {
                bineulI.Knockback();
                PlayerKnockback();
                return;
            }

            BossMonsterChiko chiko = hit.collider.GetComponent<BossMonsterChiko>();
            if (chiko != null)
            {
                chiko.IncreaseMoveSpeed();
                chiko.Knockback();
                PlayerKnockback();
                return;
            }
        }
    }

    public void PlayerKnockback()
    {
        if (!isKnockback)
        {
            Vector3 targetPosition = new Vector3(stageManager.CurrentStage.PlayerSpawnPoint.position.x, transform.position.y, transform.position.z);
            StartCoroutine(PlayerKnockbackCoroutine(targetPosition ,knockbackDuration));
        }
    }

    private IEnumerator PlayerKnockbackCoroutine(Vector3 targetPosition, float duration)
    {
        isKnockback = true;

        Vector3 startPosition = playerRb.position;
        
        float timer = 0f;

        while (timer < duration)
        {
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, timer / duration);
            playerRb.MovePosition(newPosition);
            timer += Time.deltaTime;
            yield return null;
        }
        playerRb.MovePosition(targetPosition);
        isKnockback = false;
    }

    public void OnAttack()
    {
        Vector2 direction = Vector2.right;

        RaycastHit2D[] hits = Physics2D.RaycastAll(attackPosition.position, direction, playerData.attackRange, monsterLayer);

        if (hits.Length > 0)
        {
            var hit = hits[0];

            MonsterGroup group = hit.collider.GetComponent<MonsterGroup>();
            if (group != null && group.Monsters.Count > 0)
            {
                group.Monsters[0].TakeDamage(playerData.attackPoint);
                return;
            }

            Monster monster = hit.collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(playerData.attackPoint);
                return;
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
        isHit = true;
    }

    public override void Die()
    {
        playerStateManager.ChangeState(DieState);
        OnDie?.Invoke();
    }

    public void ChangeIsHit(bool value)
    {
        isHit = value;
    }
}
