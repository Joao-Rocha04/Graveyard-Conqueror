using UnityEngine;

public class EnemyFollow2D : MonoBehaviour
{
    public float velocidade = 2f;
    private Transform alvo; // Refer�ncia ao jogador
    public ObjectPool pool; // Refer�ncia ao pool para retorno

    void Start()
    {
        // Achar o jogador pela tag
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            alvo = jogador.transform;
        }
    }

    void Update()
    {
        if (alvo != null)
        {
            // Move o inimigo em dire��o ao jogador
            Vector2 direcao = ((Vector2)alvo.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(direcao * velocidade * Time.deltaTime);
        }
    }

    // Detectar colis�o com o jogador
    void OnTriggerEnter2D(Collider2D colisor)
    {
        if (colisor.CompareTag("Player"))
        {
            if (pool != null)
            {
                pool.ReturnObjectToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}