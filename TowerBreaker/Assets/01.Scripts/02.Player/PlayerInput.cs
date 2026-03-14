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

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CheckWaiting();
            playerRb.AddForce(Vector2.right * player.PlayerData.characterData.moveSpeed, ForceMode2D.Impulse);
        }
    }

    public void OnDefence(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //레이캐스트, 일정거리(방어가능거리)까지 레이를 쏴서 몬스터 군집이 존재하면 방어하도록
            //
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CheckWaiting();
            player.AttackAnimation();
        }

        if (context.phase == InputActionPhase.Performed)
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
