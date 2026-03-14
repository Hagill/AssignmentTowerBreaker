using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private int backgroundCount;
    [SerializeField] private LayerMask backgroundLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currentGameObject = collision.gameObject;

        if (((1 << currentGameObject.layer) & backgroundLayer) != 0)
        {
            float backgroundWidth = ((BoxCollider2D)collision).size.x;
            Vector3 pos = collision.transform.position;

            pos.x += backgroundWidth * backgroundCount;
            collision.transform.position = pos;
            return;
        }
    }
}
