using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ObjectPool enemyPool;
    public float intervaloSpawn = 2f;
    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, intervaloSpawn);
    }
    void SpawnEnemy()
    {
        // Pede um inimigo do pool
        GameObject inimigo = enemyPool.RequestObjectFromPool();
        inimigo.transform.position = CalcularPosicaoSpawn();

        EnemyFollow2D comportamento = inimigo.GetComponent<EnemyFollow2D>();
        if (comportamento != null)
        {
            comportamento.pool = enemyPool;
        }

        if (inimigo != null)
        {
            // Define uma posição para spawnar o inimigo
            Vector2 posicaoSpawn = CalcularPosicaoSpawn();
            inimigo.transform.position = posicaoSpawn;
            // Podemos ajustar rotação ou outros atributos se necessário
        }
    }

    Vector2 CalcularPosicaoSpawn()
    {
        float range = 5f;
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        return new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
