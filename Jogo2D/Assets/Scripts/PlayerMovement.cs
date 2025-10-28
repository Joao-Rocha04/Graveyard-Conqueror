using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float velocidade = 5f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D ou seta esquerda/direita
        float vertical = Input.GetAxis("Vertical");   // W/S ou seta cima/baixo

        Vector2 movement = new Vector2(horizontal, vertical);
        rb.MovePosition(rb.position + movement * velocidade * Time.fixedDeltaTime);
    }
}
