using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidade = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);

        float mul = GameUpgrades.Instance ? GameUpgrades.Instance.playerSpeedMul : 1f;
        rb.linearVelocity = movement * (velocidade * mul);
        Debug.Log("Velocidade do jogador: " + rb.linearVelocity);

        if (animator != null)
        {
            bool estaCorrendo = movement.sqrMagnitude > 0.01f;
            animator.SetBool("isRunning", estaCorrendo);
        }

        if (spriteRenderer != null && Mathf.Abs(horizontal) > 0.01f)
        {
            spriteRenderer.flipX = horizontal < 0f;
        }
    }
}
