using UnityEngine;

public class EnemyFollow2D : MonoBehaviour
{
    public float velocidade = 2f;
    private Transform alvo; // Referência ao jogador
    public ObjectPool pool; // Referência ao pool para retorno

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
            // Move o inimigo em direção ao jogador
            Vector2 direcao = ((Vector2)alvo.position - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(direcao * velocidade * Time.deltaTime);
        }
    }

    // Detectar colisão com o jogador
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