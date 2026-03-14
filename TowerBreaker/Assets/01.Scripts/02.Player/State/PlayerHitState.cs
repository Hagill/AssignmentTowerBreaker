using UnityEngine;

public class PlayerHitState : CharacterHitState<Player>
{
    Player player;

    public PlayerHitState(Player character, CharacterStateManager<Player> stateManager) : base(character, stateManager)
    {
    }

    public override void EnterState()
    {
        player.PlayerRb.linearVelocity = Vector2.zero;
        player.GameManager.GameWaiting();
        stateManager.ChangeState(player.IdleState);
    }
}
