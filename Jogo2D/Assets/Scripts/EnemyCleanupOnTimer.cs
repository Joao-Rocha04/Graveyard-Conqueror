using UnityEngine;

public class EnemyCleanupOnTimer : MonoBehaviour
{
    private GameTimer gameTimer;
    private bool cleaned = false;

    void Awake()
    {
        // Tenta achar o GameTimer automaticamente na cena
        gameTimer = FindFirstObjectByType<GameTimer>();
        if (gameTimer == null)
        {
            Debug.LogError("[EnemyCleanupOnTimer] Nenhum GameTimer encontrado na cena.");
        }
    }

    void Update()
    {
        if (cleaned) return;
        if (gameTimer == null) return;

        // Quando o timer terminar, limpamos todos os inimigos
        if (gameTimer.IsFinished)
        {
            CleanupAllEnemies();
            cleaned = true;
        }
    }

    void CleanupAllEnemies()
    {
        // Pega todos os inimigos que usam EnemyFollow2D
        EnemyFollow2D[] enemies = FindObjectsOfType<EnemyFollow2D>();

        Debug.Log($"[EnemyCleanupOnTimer] Limpando {enemies.Length} inimigos.");

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            // Se estiver usando ObjectPool, devolve pra pool
            if (enemy.pool != null)
            {
                enemy.pool.ReturnObjectToPool(enemy.gameObject);
            }
            else
            {
                // fallback: desativa o inimigo
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
