using UnityEngine;

public class MonsterActiveState : CharacterActiveState<Monster>
{
    public MonsterActiveState(Monster character, CharacterStateManager<Monster> stateManager) : base(character, stateManager)
    {
    }
}
