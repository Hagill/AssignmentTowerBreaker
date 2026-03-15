using System.Collections;
using UnityEngine;
using static ConstValue;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MonsterDieState : CharacterDieState<Monster>
{
    public MonsterDieState(Monster character, CharacterStateManager<Monster> stateManager) : base(character, stateManager)
    {
    }

    public override void EnterState()
    {
        character.MonsterRb.constraints = RigidbodyConstraints2D.None;
        if (character.MonsterGroup != null)
        {
            character.MonsterGroup.Monsters.Remove(character);
            character.MonsterGroup.ResizeCollider();
        }
        character.InvokeOnMonsterDied();
        character.StartCoroutine(DieAnimation());
    }

    protected IEnumerator DieAnimation()
    {
        yield return null;
        character.Animator.SetTrigger(DieAnim);
        Vector2 direction = new Vector2(1, 1).normalized;
        character.MonsterRb.AddForce(direction * character.DieFlySpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(character.DieFlyTime);
        character.Animator.ResetTrigger(DieAnim);
        character.transform.localRotation = Quaternion.identity;
        EnemyObjectPoolManager.Instance.ReturnToPool(character.gameObject, character.OriginalPrefab);
    }
}
