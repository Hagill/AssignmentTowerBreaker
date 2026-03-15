using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    public void OnMoveKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear)
        {
            CheckWaiting();
            player.Move();
            player.ChangeIsHit(false);
        }
    }

    public void OnDefenceKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear && !player.IsHit)
        {
            CheckWaiting();
            player.Defence();
        }
    }

    public void OnAttackKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear && !player.IsHit)
        {
            CheckWaiting();
            player.AttackAnimation();
        }
    }

    public void OnFirstSkillKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear && !player.IsHit)
        {
            if (player.CurrentFirstSkillCooldown <= 0)
            {
                CheckWaiting();
                player.OnFirstSkill();
            }
        }
    }

    public void OnSecondSkillKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear && !player.IsHit)
        {
            if (player.CurrentSecondSkillCooldown <= 0)
            {
                CheckWaiting();
                player.SecondSkillAnimation();
                player.ResetSeconSkillCooldown();
            }
        }
    }

    public void OnThirdSkillKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear && !player.IsHit)
        {
            if (player.CurrentThirdSkillCooldown <= 0)
            {
                CheckWaiting();
                player.OnThirdSkill();
            }
        }
    }

    public void CheckWaiting()
    {
        if (player.GameManager.isWaiting == true)
        {
            player.GameManager.GameActive();
        }
    }
}
