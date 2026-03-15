using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveSpeed { get; protected set; }
    public float MaxHp { get; protected set; }
    public float Hp { get; protected set; }

    protected virtual void Awake() { }

    protected virtual void Start() { }

    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }

    protected void InitCharacterData(CharacterData characterData)
    {
        MoveSpeed = characterData.moveSpeed;
        MaxHp = characterData.maxHp;
        Hp = characterData.maxHp;
    }

    protected void InitCharacterData(CharacterData characterData, int stageNumber = 1)
    {
        MoveSpeed = characterData.moveSpeed;
        float increaseHp = characterData.maxHp + stageNumber;
        MaxHp = increaseHp;
        Hp = increaseHp;
    }

    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }
    }

    public virtual void Die() { }
}
