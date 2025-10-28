using UnityEngine;

public class CameraMover2D : MonoBehaviour
{
    public float speed = 8f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new(h, v, 0f);
        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
