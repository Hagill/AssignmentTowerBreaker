using UnityEngine;

public class PlayerHitState : CharacterHitState<Player>
{
    public PlayerHitState(Player character, CharacterStateManager<Player> stateManager) : base(character, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }
}
