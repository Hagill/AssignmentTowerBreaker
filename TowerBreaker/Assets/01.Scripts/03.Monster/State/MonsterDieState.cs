using System.Collections;
using UnityEngine;

public class MonsterDieState : CharacterDieState<Monster>
{
    public MonsterDieState(Monster character, CharacterStateManager<Monster> stateManager) : base(character, stateManager)
    {
    }

    public override void EnterState()
    {
        character.MonsterRb.constraints = RigidbodyConstraints2D.None;
        character.MonsterGroup.Monsters.Remove(character);
        character.MonsterGroup.ResizeCollider();
        character.StartCoroutine(DieAnimation());
    }

    protected IEnumerator DieAnimation()
    {
        yield return null;

        Vector2 direction = new Vector2(1, 1).normalized;
        character.MonsterRb.AddForce(direction * character.DieFlySpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(character.DieFlyTime);
        Object.Destroy(character.gameObject);
    }
}
