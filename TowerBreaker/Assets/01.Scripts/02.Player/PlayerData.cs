using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public CharacterData characterData;
    public float attackPoint;   // 공격력
    public float knockBackValue;// 방어시 넉백 수치
    public float attackCooldown;   // 공격 쿨타임(공격속도)
    public float attackRange;   // 공격 사정거리
    public float defenceRange;  // 방어 사정거리
}
