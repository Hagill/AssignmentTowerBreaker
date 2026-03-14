using UnityEngine;

public class PlayerIdleState : CharacterIdleState<Player>
{
    public PlayerIdleState(Player character, CharacterStateManager<Player> stateManager) : base(character, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }
}
