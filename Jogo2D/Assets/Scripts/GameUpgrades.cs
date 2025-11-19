using UnityEngine;

public class GameUpgrades : MonoBehaviour
{
    public static GameUpgrades Instance { get; private set; }

    // ===== Multiplicadores globais existentes =====
    [Header("Player / Inimigos")]
    public float playerSpeedMul = 1f;
    public float enemySpeedMul  = 1f;

    public float playerSpeedStep = 1.10f; // +10% por pick
    public float enemySlowStep   = 0.90f; // -10% por pick

    // ===== Novos upgrades =====

    [Header("Sunstrike - Dano")]
    public float sunstrikeDamageMul = 1f;
    public float sunstrikeDamageStep = 1.25f; // +25% por pick

    [Header("Sunstrike - Projéteis extras")]
    public int sunstrikeExtraProjectiles = 0;
    public int sunstrikeProjectilesStep = 1; // +1 alvo por pick

    [Header("Sunstrike - Alcance")]
    public float playerRangeMul = 1f;
    public float playerRangeStep = 1.15f; // +15% no raio por pick

    [Header("XP ganho")]
    public float xpGainMul = 1f;
    public float xpGainStep = 1.20f; // +20% XP por pick

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ===== Upgrades existentes =====

    public void Upgrade_PlayerSpeed()
    {
        playerSpeedMul *= playerSpeedStep;
    }

    public void Upgrade_EnemySlow()
    {
        enemySpeedMul *= enemySlowStep;

        // aplica nos inimigos já ativos
        foreach (var e in FindObjectsByType<EnemyFollow2D>(
                     FindObjectsInactive.Exclude,
                     FindObjectsSortMode.None))
        {
            e.ApplySpeedMultiplier(enemySpeedMul);
        }
    }

    // ===== Novos upgrades =====

    public void Upgrade_SunstrikeDamage()
    {
        sunstrikeDamageMul *= sunstrikeDamageStep;
    }

    public void Upgrade_SunstrikeMoreProjectiles()
    {
        sunstrikeExtraProjectiles += sunstrikeProjectilesStep;
    }

    public void Upgrade_PlayerRange()
    {
        playerRangeMul *= playerRangeStep;

        // aumenta o radius do collider de alcance
        PlayerSunstrike ps = FindFirstObjectByType<PlayerSunstrike>();
        if (ps != null)
        {
            var col = ps.GetComponent<CircleCollider2D>();
            if (col != null)
            {
                col.radius *= playerRangeStep;
            }
        }
    }

    public void Upgrade_XPGain()
    {
        xpGainMul *= xpGainStep;
    }
}
