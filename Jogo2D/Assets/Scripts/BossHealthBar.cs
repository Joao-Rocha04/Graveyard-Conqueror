using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    private BossHealth bossHealth;

    void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
    }

    void Start()
    {
        // acha o BossHealth na cena
        bossHealth = FindFirstObjectByType<BossHealth>();

        if (bossHealth == null)
        {
            Debug.LogWarning("[BossHealthBar] Nenhum BossHealth encontrado na cena.");
            enabled = false;
            return;
        }

        // inicializa valores
        slider.maxValue = bossHealth.maxHealth;
        slider.value = bossHealth.currentHealth;

        // inscreve no evento de mudança de vida
        bossHealth.OnHealthChanged += AtualizarBarra;
    }

    void OnDestroy()
    {
        if (bossHealth != null)
            bossHealth.OnHealthChanged -= AtualizarBarra;
    }

    private void AtualizarBarra(int vidaAtual, int vidaMax)
    {
        slider.maxValue = vidaMax;
        slider.value = vidaAtual;
    }
}
