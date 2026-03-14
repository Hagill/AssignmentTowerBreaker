using UnityEngine;

public class MonsterIdleState : CharacterIdleState<Monster>
{
    private Monster monster;
    private Rigidbody2D monsterRb;

    public MonsterIdleState(Monster monster, CharacterStateManager<Monster> monsterStateManager) : base(monster, monsterStateManager)
    {
    }

    public override void EnterState()
    {
        monsterRb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
        if (GameManager.Instance.isWaiting == false)
        {
            stateManager.ChangeState(monster.ActiveState);
        }
    }
}