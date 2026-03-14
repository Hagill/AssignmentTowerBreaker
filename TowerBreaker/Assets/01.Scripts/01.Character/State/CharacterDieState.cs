public class CharacterDieState<T> : CharacterState<T> where T : Character
{
    public CharacterDieState(T character, CharacterStateManager<T> stateManager) : base(character, stateManager)
    {
    }
}
