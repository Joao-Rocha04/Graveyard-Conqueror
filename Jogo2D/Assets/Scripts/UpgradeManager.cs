using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public Button btnOption1;
    public Button btnOption2;
    public TMP_Text txtOption1;
    public TMP_Text txtOption2;

    bool open;

    private enum UpgradeType
    {
        PlayerSpeed,
        EnemySlow,
        SunstrikeDamage,
        SunstrikeMoreProjectiles,
        PlayerRange,
        XPGain
    }

    private readonly UpgradeType[] allUpgrades = new UpgradeType[]
    {
        UpgradeType.PlayerSpeed,
        UpgradeType.EnemySlow,
        UpgradeType.SunstrikeDamage,
        UpgradeType.SunstrikeMoreProjectiles,
        UpgradeType.PlayerRange,
        UpgradeType.XPGain
    };

    private UpgradeType option1Type;
    private UpgradeType option2Type;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        // tecla U para testar manualmente, se quiser
        if (Input.GetKeyDown(KeyCode.U))
            Toggle();
    }

    // Chamado pelo PlayerXP ao upar
    public void ShowOnLevelUp()
    {
        SortearOpcoes();
        Open();
    }

    public void Toggle()
    {
        if (open) Close();
        else
        {
            SortearOpcoes();
            Open();
        }
    }

    void Open()
    {
        open = true;
        if (panel != null)
            panel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    void Close()
    {
        open = false;
        if (panel != null)
            panel.SetActive(false);

        Time.timeScale = 1f;
    }

    void SortearOpcoes()
    {
        if (allUpgrades.Length < 2)
        {
            Debug.LogError("[UpgradeManager] Menos de 2 upgrades configurados.");
            return;
        }

        int idx1 = Random.Range(0, allUpgrades.Length);
        int idx2;
        do
        {
            idx2 = Random.Range(0, allUpgrades.Length);
        } while (idx2 == idx1);

        option1Type = allUpgrades[idx1];
        option2Type = allUpgrades[idx2];

        if (txtOption1 != null)
            txtOption1.text = GetUpgradeName(option1Type);
        if (txtOption2 != null)
            txtOption2.text = GetUpgradeName(option2Type);
    }

    string GetUpgradeName(UpgradeType t)
    {
        switch (t)
        {
            case UpgradeType.PlayerSpeed:              return "Velocidade do Jogador +";
            case UpgradeType.EnemySlow:                return "Inimigos Mais Lentos";
            case UpgradeType.SunstrikeDamage:          return "Dano do Sunstrike +";
            case UpgradeType.SunstrikeMoreProjectiles: return "Mais Projéteis";
            case UpgradeType.PlayerRange:              return "Alcance do Sunstrike +";
            case UpgradeType.XPGain:                   return "XP Ganhado +";
        }
        return t.ToString();
    }

    // ==== Chamados pelos botões via Inspector ====

    public void OnButton1Click()
    {
        Debug.Log("[UpgradeManager] Clique no botão 1");
        ApplyUpgrade(option1Type);
        Close();
    }

    public void OnButton2Click()
    {
        Debug.Log("[UpgradeManager] Clique no botão 2");
        ApplyUpgrade(option2Type);
        Close();
    }

    void ApplyUpgrade(UpgradeType t)
    {
        if (GameUpgrades.Instance == null)
        {
            Debug.LogError("[UpgradeManager] GameUpgrades.Instance é nulo.");
            return;
        }

        switch (t)
        {
            case UpgradeType.PlayerSpeed:
                GameUpgrades.Instance.Upgrade_PlayerSpeed();
                break;

            case UpgradeType.EnemySlow:
                GameUpgrades.Instance.Upgrade_EnemySlow();
                break;

            case UpgradeType.SunstrikeDamage:
                GameUpgrades.Instance.Upgrade_SunstrikeDamage();
                break;

            case UpgradeType.SunstrikeMoreProjectiles:
                GameUpgrades.Instance.Upgrade_SunstrikeMoreProjectiles();
                break;

            case UpgradeType.PlayerRange:
                GameUpgrades.Instance.Upgrade_PlayerRange();
                break;

            case UpgradeType.XPGain:
                GameUpgrades.Instance.Upgrade_XPGain();
                break;
        }
    }
}
