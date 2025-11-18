using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PlayerXP : MonoBehaviour
{
    [Header("XP")]
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 15;
    public float xpGrowth = 1.4f;

    [Header("Upgrades")]
    public int minLevelForUpgrades = 3;
    public int levelsPerUpgrade = 1;

    [Header("UI (opcional)")]
    public Slider xpSlider;   // <-- AQUI
    public TMP_Text levelText;    // <-- E AQUI

    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindFirstObjectByType<UpgradeManager>();
        AtualizarUI();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;

        int oldLevel = level;
        bool ganhouPeloMenosUmNivel = false;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            InternalLevelUp();
            ganhouPeloMenosUmNivel = true;
        }

        bool deveMostrarUpgrade = false;

        if (ganhouPeloMenosUmNivel && upgradeManager != null)
        {
            for (int l = oldLevel + 1; l <= level; l++)
            {
                if (l >= minLevelForUpgrades &&
                    ((l - minLevelForUpgrades) % levelsPerUpgrade) == 0)
                {
                    deveMostrarUpgrade = true;
                    break;
                }
            }
        }

        if (deveMostrarUpgrade && upgradeManager != null)
        {
            upgradeManager.ShowOnLevelUp();
        }

        AtualizarUI();
    }

    private void InternalLevelUp()
    {
        level++;
        xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * xpGrowth);
    }

    private void AtualizarUI()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }

        if (levelText != null)
        {
            levelText.text = "Lv " + level;
        }
    }
}
