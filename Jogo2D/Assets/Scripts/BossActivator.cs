using UnityEngine;

public class BossActivator : MonoBehaviour
{
    private GameTimer gameTimer;
    private BossBehavior boss;
    private Transform spawnPoint;

    // UI da barra de vida do boss
    private GameObject bossHealthUI;

    private bool bossMovedToArena = false;

    void Awake()
    {
        // Acha o GameTimer
        gameTimer = FindFirstObjectByType<GameTimer>();
        if (gameTimer == null)
        {
            Debug.LogError("[BossActivator] Nenhum GameTimer encontrado na cena.");
        }

        // Acha o Boss (qualquer objeto com BossBehavior)
        boss = FindFirstObjectByType<BossBehavior>();
        if (boss == null)
        {
            Debug.LogError("[BossActivator] Nenhum BossBehavior encontrado na cena.");
        }
        else
        {
            // Esconde o Boss fora da tela até a hora da luta
            boss.transform.position = new Vector3(9999f, 9999f, 0f);
        }

        // Acha o ponto de spawn pelo nome
        GameObject sp = GameObject.Find("BossSpawnPoint");
        if (sp != null)
        {
            spawnPoint = sp.transform;
        }
        else
        {
            Debug.LogWarning("[BossActivator] BossSpawnPoint não encontrado. Vai usar (0,0,0) como posição.");
        }

        // Acha a barra de vida do boss pelo nome
        bossHealthUI = GameObject.Find("BossHealthSlider");   // nome do GameObject da UI
        if (bossHealthUI != null)
        {
            // garante que começa desligada
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

        // Quando o timer terminar, traz o Boss e mostra a barra
        if (gameTimer.IsFinished)
        {
            Vector3 targetPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
            boss.transform.position = targetPos;
            bossMovedToArena = true;

            // Ativa a barra de vida do boss
            if (bossHealthUI != null)
            {
                bossHealthUI.SetActive(true);
            }

            Debug.Log("[BossActivator] Boss movido para a arena e barra de vida ativada.");
        }
    }
}
