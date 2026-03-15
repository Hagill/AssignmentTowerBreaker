using UnityEngine;

public class MonsterIdleState : CharacterIdleState<Monster>
{
    public MonsterIdleState(Monster monster, CharacterStateManager<Monster> monsterStateManager) : base(monster, monsterStateManager)
    {
    }

    public override void EnterState()
    {
    }
}