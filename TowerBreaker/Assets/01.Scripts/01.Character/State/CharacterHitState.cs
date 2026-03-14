public class CharacterHitState<T> : CharacterState<T> where T : Character
{
    public CharacterHitState(T character, CharacterStateManager<T> stateManager) : base(character, stateManager)
    {
    }
}
