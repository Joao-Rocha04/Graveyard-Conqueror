using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidade = 5f;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D ou setas
        float vertical = Input.GetAxis("Vertical");     // W/S ou setas

        Vector2 movement = new Vector2(horizontal, vertical);

        float mul = GameUpgrades.Instance ? GameUpgrades.Instance.playerSpeedMul : 1f;
        rb.linearVelocity = movement * (velocidade * mul);

        // Ativa/desativa animação de corrida
        if (animator != null)
        {
            bool estaCorrendo = movement.sqrMagnitude > 0.01f;
            animator.SetBool("isRunning", estaCorrendo);
        }
    }
}
