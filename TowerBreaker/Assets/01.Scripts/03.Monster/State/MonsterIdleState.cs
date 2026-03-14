using UnityEngine;

public class MonsterIdleState<T> : CharacterState<T> where T : Character
{
    public MonsterIdleState(T character, CharacterStateManager<T> stateManager) : base(character, stateManager)
    {
    }
}
