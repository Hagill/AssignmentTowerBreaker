using UnityEngine;

public class Monster : Character
{
    private CharacterStateManager<Monster> monsterStateManager;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private Animator animator;

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
}
