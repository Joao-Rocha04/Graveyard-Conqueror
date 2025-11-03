using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool enemyPool;
    public float intervaloSpawn = 2f;
    public float range = 5f;

    void Start()
    {
        if (enemyPool == null)
        {
            Debug.LogError("[EnemySpawner] enemyPool nao atribuido.");
            return;
        }
        InvokeRepeating(nameof(SpawnEnemy), 1f, intervaloSpawn);
    }

    void SpawnEnemy()
    {
        if (enemyPool == null) return;

        // Pede um inimigo do pool
        GameObject inimigo = enemyPool.RequestObjectFromPool();
        if (inimigo == null) return;

        // Posiciona
        inimigo.transform.position = CalcularPosicaoSpawn();

        // Configura comportamento e aplica multiplicador vigente
        var comportamento = inimigo.GetComponent<EnemyFollow2D>();
        if (comportamento != null)
        {
            comportamento.pool = enemyPool;

            float mul = GameUpgrades.Instance ? GameUpgrades.Instance.enemySpeedMul : 1f;
            comportamento.ApplySpeedMultiplier(mul);
        }
    }

    Vector2 CalcularPosicaoSpawn()
    {
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        return new Vector2(x, y);
    }

    void Update() { }
}
