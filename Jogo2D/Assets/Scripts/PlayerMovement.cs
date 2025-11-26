using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogWarning("PlayerMovement: Nenhuma câmera com tag MainCamera encontrada.");
        }
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;

        bool temToque = false;
        Vector3 touchWorld = Vector3.zero;

        // 1) TOQUE REAL (CELULAR)
        if (Input.touchCount > 0 && mainCam != null)
        {
            Touch touch = Input.GetTouch(0);
            touchWorld = mainCam.ScreenToWorldPoint(touch.position);
            touchWorld.z = 0f;
            temToque = true;
        }
        // 2) "TOQUE" SIMULADO COM MOUSE (PC)
        else if (Input.GetMouseButton(0) && mainCam != null)
        {
            touchWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            touchWorld.z = 0f;
            temToque = true;
        }

        // Se tiver toque (real ou mouse), anda na direção do ponto
        if (temToque)
        {
            Vector2 dir = (touchWorld - transform.position);

            // Só se mover se estiver um pouquinho longe do ponto
            if (dir.magnitude > 0.1f)
            {
                movement = dir.normalized;
            }
        }
        else
        {
            // Sem toque: usa teclado (bom pra testar no Editor)
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            movement = new Vector2(horizontal, vertical).normalized;
        }

        // Aplica velocidade
        float mul = GameUpgrades.Instance ? GameUpgrades.Instance.playerSpeedMul : 1f;
        rb.linearVelocity = movement * (velocidade * mul);
        Debug.Log("Velocidade do jogador: " + rb.linearVelocity);

        // Animação
        if (animator != null)
        {
            bool estaCorrendo = movement.sqrMagnitude > 0.01f;
            animator.SetBool("isRunning", estaCorrendo);
        }

        // Flip do sprite
        if (spriteRenderer != null && Mathf.Abs(movement.x) > 0.01f)
        {
            spriteRenderer.flipX = movement.x < 0f;
        }
    }
}
