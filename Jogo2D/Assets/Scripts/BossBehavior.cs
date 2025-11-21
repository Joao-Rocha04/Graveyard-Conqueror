using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossBehavior : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 2f;          // velocidade correndo atrás do player
    public float attackRange = 1.2f;  // distância para começar a atacar
    public float stopBuffer = 0.1f;   // folga pra não ficar liga/desliga ataque

    [Header("Ataque")]
    [SerializeField] private int totalAttacks = 4;    // Attack1..AttackN no Animator
    [SerializeField] private float attackCooldown = 1.2f;

    [Header("Visual")]
    public bool spriteFacesRight = true; // se o sprite original olha pra direita
    public bool debugLogs = false;

    private Animator animator;
    private SpriteRenderer sr;
    private Transform player;
    private float lastAttackTime = -999f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // tenta achar o player por tag
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
        else
        {
            if (debugLogs)
                Debug.LogWarning("[BossBehavior] Nenhum Player com tag 'Player' encontrado.");
        }
    }

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            if (player == null) return;
        }

        Vector3 dir = player.position - transform.position;
        float dist = dir.magnitude;

        bool dentroDoAtaque = dist <= attackRange;
        float speedParam = 0f;

        // --------- Movimento até o player ---------
        if (!dentroDoAtaque)
        {
            Vector3 moveDir = dir.normalized;
            transform.position += moveDir * speed * Time.deltaTime;
            speedParam = speed;
        }

        // --------- Flip do sprite ---------
        if (dir.x != 0f)
        {
            bool olhandoPraDireita = !(dir.x > 0f);
            bool flip;

            if (spriteFacesRight)
                flip = !olhandoPraDireita;
            else
                flip = olhandoPraDireita;

            sr.flipX = flip;
        }

        // --------- Parâmetro de movimento no Animator ---------
        if (animator != null)
        {
            animator.SetFloat("Speed", dentroDoAtaque ? 0f : speedParam);
        }

        // --------- Ataque ---------
        if (dentroDoAtaque && Time.time >= lastAttackTime + attackCooldown)
        {
            DispararAtaqueAleatorio();
            lastAttackTime = Time.time;
        }
    }

    void DispararAtaqueAleatorio()
    {
        if (animator == null || totalAttacks <= 0)
            return;

        int escolha = Random.Range(1, totalAttacks + 1);
        string triggerName = "Attack" + escolha;

        // limpa triggers antigos
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Attack4");

        animator.SetTrigger(triggerName);

        if (debugLogs)
            Debug.Log($"[BossBehavior] Disparou {triggerName}");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
