using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool enemyPool;
    public float intervaloSpawn = 2f;
    public float rangeMax = 10f;      // Raio máximo de spawn
    public float rangeMin = 5f;       // Raio mínimo de spawn (donut)

    private Transform jogadorTransform;
    private GameTimer gameTimer;

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
        // spawnar conforme o timer do jogo


        gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer == null)
        {
            Debug.LogError("[EnemySpawner] GameTimer não encontrado na cena.");
            return;
        }

        StartCoroutine(SpawnComIntervaloDinamico());
    }
    private IEnumerator SpawnComIntervaloDinamico()
    {
        while (!gameTimer.IsFinished)
        {
            SpawnEnemy();

            float t = Mathf.Clamp01(1f - (gameTimer.currentTime / gameTimer.totalTimeSeconds));
            float intervaloAtual = Mathf.Lerp(3f, 0.05f, t); // De 3s até 0.2s conforme o tempo passa

            yield return new WaitForSeconds(intervaloAtual);
        }
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
        Vector2 centro = jogadorTransform.position;
        Vector2 spawnPos;

        // Gera posição aleatória em um anel (donut) com distância mínima e máxima
        do
        {
            Vector2 offset = Random.insideUnitCircle * rangeMax;
            spawnPos = centro + offset;
        } while (Vector2.Distance(spawnPos, centro) < rangeMin);

        return spawnPos;
    }

    void Update() { }
}
