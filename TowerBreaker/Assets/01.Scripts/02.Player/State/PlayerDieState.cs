using UnityEngine;

public class PlayerDieState : CharacterDieState<Player>
{
    public PlayerDieState(Player character, CharacterStateManager<Player> stateManager) : base(character, stateManager)
    {
    }

    public override void EnterState()
    {
        character.GameManager.GameOver();
        character.GameSceneManager.ShowGameOverPopup();
    }
}
