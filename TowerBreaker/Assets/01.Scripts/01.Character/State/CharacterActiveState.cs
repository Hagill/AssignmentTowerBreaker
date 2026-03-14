public class CharacterActiveState<T> : CharacterState<T> where T : Character
{
    public CharacterActiveState(T character, CharacterStateManager<T> stateManager) : base(character, stateManager)
    {
    }
}
