using UnityEngine;

public class PlayerActiveState : CharacterActiveState<Player>
{
    public PlayerActiveState(Player character, CharacterStateManager<Player> stateManager) : base(character, stateManager)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }
}
