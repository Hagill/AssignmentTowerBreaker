using UnityEngine;

public class Item : MonoBehaviour
{
    private float itemAttackPoint;

    public float ItemAttackPoint => itemAttackPoint;

    public void SetAttackPoint(float attackPoint)
    {
        itemAttackPoint = attackPoint;
    }
}
