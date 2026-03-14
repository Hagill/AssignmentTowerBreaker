using UnityEngine;

public class PlayerHitState : CharacterHitState<Player>
{
    Player player;

    public PlayerHitState(Player character, CharacterStateManager<Player> stateManager) : base(character, stateManager)
    {
        player = character;
    }

    public override void EnterState()
    {
        player.PlayerRb.linearVelocity = Vector2.zero;
        GameManager.Instance.GameWaiting();
        stateManager.ChangeState(player.IdleState);
    }
}
