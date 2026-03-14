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
        }
    }

    public void OnDefenceKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear)
        {
            //레이캐스트, 일정거리(방어가능거리)까지 레이를 쏴서 몬스터 군집이 존재하면 방어하도록
            //
        }
    }

    public void OnAttackKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !GameManager.Instance.isStageClear)
        {
            CheckWaiting();
            player.AttackAnimation();
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
