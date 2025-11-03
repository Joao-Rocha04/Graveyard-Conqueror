using UnityEngine;

public class GameUpgrades : MonoBehaviour
{
    public static GameUpgrades Instance { get; private set; }

    // multiplicadores globais aplicados em tempo de jogo
    public float playerSpeedMul = 1f;
    public float enemySpeedMul  = 1f;

    // passos por escolha
    public float playerSpeedStep = 1.20f; // +20% por pick
    public float enemySlowStep   = 0.85f; // -15% por pick (multiplicativo)

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Upgrade_PlayerSpeed()
    {
        playerSpeedMul *= playerSpeedStep;
    }

    public void Upgrade_EnemySlow()
    {
        enemySpeedMul *= enemySlowStep;

        // aplica nos inimigos já ativos
        foreach (var e in FindObjectsByType<EnemyFollow2D>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            e.ApplySpeedMultiplier(enemySpeedMul);
    }
}
