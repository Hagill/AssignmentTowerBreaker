using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static ConstValue;

public class Player : Character
{
    private GameManager gameManager;
    private CameraShaker cameraShaker;
    private InventoryManager inventoryManager;
    private CharacterStateManager<Player> playerStateManager;
    [SerializeField] private GameSceneManager gameSceneManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask monsterLayer;    // 몬스터 레이어
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private Transform attackPosition;  // 공격 시작 위치
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakePower;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float firstSkillCooldown;
    [SerializeField] private float firstSkillDuration;
    [SerializeField] private float secondSkillCooldown;
    [SerializeField] private float thirdSkillCooldown;

    private Item equippedItem;
    private float equipAttackPoint;

    private float currentFirstSkillCooldown;
    private float currentSecondSkillCooldown;
    private float currentThirdSkillCooldown;

    private bool isKnockback;

    private bool isHit;

    private Coroutine firstSkillCoroutine;

    public event Action OnDie;

    public GameSceneManager GameSceneManager => gameSceneManager;
    public GameManager GameManager => gameManager;
    public PlayerData PlayerData => playerData;
    public Rigidbody2D PlayerRb => playerRb;
    public float FirstSkillCooldown => firstSkillCooldown;
    public float SecondSkillCooldown => secondSkillCooldown;
    public float ThirdSkillCooldown => thirdSkillCooldown;
    public float CurrentFirstSkillCooldown => currentFirstSkillCooldown;
    public float CurrentSecondSkillCooldown => currentSecondSkillCooldown;
    public float CurrentThirdSkillCooldown => currentThirdSkillCooldown;
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
        inventoryManager = InventoryManager.Instance;
        cameraShaker = Camera.main.GetComponent<CameraShaker>();
        playerStateManager.ChangeState(IdleState);
        gameSceneManager.ChangeHp(Hp);
        currentFirstSkillCooldown = firstSkillCooldown;
        currentSecondSkillCooldown = secondSkillCooldown;
        currentThirdSkillCooldown = thirdSkillCooldown;

        equippedItem = inventoryManager.GetEquippedItem();
        if (equippedItem != null)
        {
            SumEquipmentAttackPoint(equippedItem.ItemAttackPoint);
        }
    }

    private void OnDisable()
    {
        OnDie = null;
    }

    protected override void Update()
    {
        playerStateManager.Update();

        if (currentFirstSkillCooldown >= 0)
        {
            currentFirstSkillCooldown -= Time.deltaTime;
        }
        else
        {
            currentFirstSkillCooldown = 0f;
        }

        if (currentSecondSkillCooldown >= 0)
        {
            currentSecondSkillCooldown -= Time.deltaTime;
        }
        else
        {
            currentSecondSkillCooldown = 0f;
        }

        if (currentThirdSkillCooldown >= 0)
        {
            currentThirdSkillCooldown -= Time.deltaTime;
        }
        else
        {
            currentThirdSkillCooldown = 0f;
        }
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
        else if(((1<<currentGameObject.layer) & itemLayer) != 0)
        {
            Item currentItem = currentGameObject.GetComponent<Item>();
            
            if (currentItem != null)
            {
                InventoryManager.Instance.AddItem(currentItem);
            }
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
                group.Monsters[0].TakeDamage(playerData.attackPoint);//equipAttackPoint
                return;
            }

            Monster monster = hit.collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(playerData.attackPoint);//equipAttackPoint
                return;
            }
        }
    }

    // 지속시간동안 보스몬스터를 제외한 몬스터의 움직임을 멈추는 스킬
    public void OnFirstSkill()
    {
        if (firstSkillCoroutine != null)
        {
            StopCoroutine(firstSkillCoroutine);
        }

        firstSkillCoroutine = StartCoroutine(FirstSkillCoroutine());
    }

    private IEnumerator FirstSkillCoroutine()
    {
        GameManager.SetMonstersMove(true);
        currentFirstSkillCooldown = firstSkillCooldown;
        yield return new WaitForSeconds(firstSkillDuration);
        GameManager.SetMonstersMove(false);
    }

    // 광역 공격 스킬
    public void OnSecondSkill()
    {
        Vector2 direction = Vector2.right;

        RaycastHit2D[] hits = Physics2D.RaycastAll(attackPosition.position, direction, playerData.secondSkillRange, monsterLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                MonsterGroup group = hit.collider.GetComponent<MonsterGroup>();
                if (group != null && group.Monsters.Count > 0)
                {
                    for (int i = 0; i < group.Monsters.Count; i++)
                    {
                        group.Monsters[i].TakeDamage(playerData.attackPoint);//equipAttackPoint
                    }
                }

                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(playerData.attackPoint);//equipAttackPoint
                }
            }
        }
    }

    // 다른 두 개의 스킬 쿨타임을 초기화하는 스킬
    public void OnThirdSkill()
    {
        currentThirdSkillCooldown = thirdSkillCooldown;

        if (currentFirstSkillCooldown > 0)
        {
            currentFirstSkillCooldown = 0f;
        }
        
        if (currentSecondSkillCooldown > 0)
        {
            currentSecondSkillCooldown = 0f;
        }
    }

    public void ResetSeconSkillCooldown()
    {
        currentSecondSkillCooldown = secondSkillCooldown;
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(AttackAnim);
    }

    public void SecondSkillAnimation()
    {
        animator.SetTrigger(SecondSkillAnim);
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

    public void SumEquipmentAttackPoint(float equipmentAttackPoint)
    {
        equipAttackPoint = equipmentAttackPoint + playerData.attackPoint;
    }
}
