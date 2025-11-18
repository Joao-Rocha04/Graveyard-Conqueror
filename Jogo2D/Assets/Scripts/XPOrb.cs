using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [Header("XP")]
    public int xpAmount = 1;

    [Header("Coleta")]
    [Tooltip("Distância para o player coletar a orb instantaneamente.")]
    public float pickupDistance = 0.35f;

    [Header("Ímã")]
    [Tooltip("Distância máxima em que a orb começa a ser puxada. Deve ser MAIOR que pickupDistance.")]
    public float attractionRadius = 1.2f;
    [Tooltip("Velocidade com que a orb se move em direção ao player quando está no raio de ímã.")]
    public float moveSpeed = 4f;

    [Header("Tempo de vida")]
    [Tooltip("Tempo em segundos até a orb sumir sozinha. 0 ou negativo = nunca some.")]
    public float lifeTime = 10f;

    private float lifeTimer;
    private PlayerXP playerXP;
    private Transform playerTransform;

    void Start()
    {
        playerXP = FindFirstObjectByType<PlayerXP>();
        if (playerXP != null)
        {
            playerTransform = playerXP.transform;
        }
        else
        {
            Debug.LogWarning("[XPOrb] Nenhum PlayerXP encontrado na cena.");
        }

        lifeTimer = lifeTime;
    }

    void Update()
    {
        // 1) Contagem regressiva do tempo de vida
        if (lifeTime > 0f)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                Destroy(gameObject);
                return;
            }
        }

        // 2) Lógica de ímã + coleta
        if (playerXP == null || playerTransform == null)
            return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);

        // Coleta se estiver bem perto
        if (dist <= pickupDistance)
        {
            Collect();
            return;
        }

        // Ímã se estiver dentro do raio
        if (attractionRadius > pickupDistance && dist <= attractionRadius)
        {
            Vector3 dir = (playerTransform.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void Collect()
    {
        if (playerXP == null) return;

        Debug.Log($"[XPOrb] Coletada! XP: {xpAmount}");
        playerXP.AddXP(xpAmount);
        Destroy(gameObject);
    }
}
