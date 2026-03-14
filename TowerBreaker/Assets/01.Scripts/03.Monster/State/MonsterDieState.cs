using UnityEngine;

public class MonsterDieState : CharacterActiveState<Monster>
{
    public MonsterDieState(Monster character, CharacterStateManager<Monster> stateManager) : base(character, stateManager)
    {
    }
}
