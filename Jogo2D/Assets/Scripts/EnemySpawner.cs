using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool enemyPool;
    public float intervaloSpawn = 2f;
    public float range = 10f;

    private Transform jogadorTransform;

    void Start()
    {
        if (enemyPool == null)
        {
            Debug.LogError("[EnemySpawner] enemyPool não atribuído.");
            return;
        }

        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            jogadorTransform = jogador.transform;
        }
        else
        {
            Debug.LogError("[EnemySpawner] Jogador com tag 'Player' não encontrado.");
            return;
        }

        InvokeRepeating(nameof(SpawnEnemy), 1f, intervaloSpawn);
    }

    void SpawnEnemy()
    {
        if (enemyPool == null || jogadorTransform == null) return;

        GameObject inimigo = enemyPool.RequestObjectFromPool();
        if (inimigo == null) return;

        inimigo.transform.position = CalcularPosicaoSpawn();

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
        return (Vector2)jogadorTransform.position + new Vector2(x, y);
    }

    void Update() { }
}
