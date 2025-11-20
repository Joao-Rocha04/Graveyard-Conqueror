using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossBehavior : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 2f;          // velocidade de corrida atrás do player
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
            // tenta achar de novo se algo mudou
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
            if (player == null) return;
        }

        Vector3 dir = player.position - transform.position;
        float dist = dir.magnitude;

        bool dentroDoAtaque = dist <= attackRange;
        float speedParam = 0f;

        // ----------------- Movimento -----------------
        if (!dentroDoAtaque)
        {
            Vector3 moveDir = dir.normalized;
            transform.position += moveDir * speed * Time.deltaTime;
            speedParam = speed; // qualquer valor > 0 serve pro Animator
        }

        // ----------------- Flip do sprite -----------------
        if (dir.x != 0f)
        {
            bool olhandoPraDireita = !(dir.x > 0f);

            // se o sprite original já olha pra direita:
            //  - quando queremos olhar pra direita, flipX = false
            //  - quando queremos olhar pra esquerda, flipX = true
            bool flip;
            if (spriteFacesRight)
                flip = !olhandoPraDireita;
            else
                flip = olhandoPraDireita;

            sr.flipX = flip;
        }

        // parámetro de movimento no Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", dentroDoAtaque ? 0f : speedParam);
        }

        // ----------------- Ataque -----------------
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

        // limpa triggers antigos pra evitar bagunça
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
        // só pra visualizar o range de ataque na Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
