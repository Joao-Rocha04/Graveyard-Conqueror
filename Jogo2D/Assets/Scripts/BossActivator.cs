using UnityEngine;

public class BossActivator : MonoBehaviour
{
    private GameTimer gameTimer;
    private BossBehavior boss;
    private Transform spawnPoint;

    private GameObject bossHealthUI;

    private bool bossMovedToArena = false;

    void Awake()
    {
        // GameTimer
        gameTimer = FindFirstObjectByType<GameTimer>();
        if (gameTimer == null)
        {
            Debug.LogError("[BossActivator] Nenhum GameTimer encontrado na cena.");
        }

        // Boss
        boss = FindFirstObjectByType<BossBehavior>();
        if (boss == null)
        {
            Debug.LogError("[BossActivator] Nenhum BossBehavior encontrado na cena.");
        }
        else
        {
            // esconde o boss fora da tela até a luta
            boss.transform.position = new Vector3(9999f, 9999f, 0f);
        }

        // Ponto de spawn
        GameObject sp = GameObject.Find("BossSpawnPoint");
        if (sp != null)
        {
            spawnPoint = sp.transform;
        }
        else
        {
            Debug.LogWarning("[BossActivator] BossSpawnPoint não encontrado. Vai usar (0,0,0).");
        }

        // UI da vida do boss
        bossHealthUI = GameObject.Find("BossHealthBar");
        if (bossHealthUI != null)
        {
            // começa desativada via script
            bossHealthUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[BossActivator] BossHealthSlider não encontrado na cena.");
        }
    }

    void Update()
    {
        if (bossMovedToArena) return;
        if (gameTimer == null || boss == null) return;

        if (gameTimer.IsFinished)
        {
            Vector3 targetPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
            boss.transform.position = targetPos;
            bossMovedToArena = true;

            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(true);
            }

            Debug.Log("[BossActivator] Boss movido para arena e barra de vida ativada.");
        }
    }
}
