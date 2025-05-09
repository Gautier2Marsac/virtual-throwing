using UnityEngine;

public class LockYPosition : MonoBehaviour
{
    public float fixedY = 3f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }
}
