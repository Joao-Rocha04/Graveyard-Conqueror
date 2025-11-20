using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private Slider slider;

    void Awake()
    {
        // pega o Slider desse GameObject
        slider = GetComponent<Slider>();

        // pega o PlayerHealth na cena usando a API nova
        // (procura o primeiro objeto com PlayerHealth)
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogWarning("[PlayerHealthBar] Nenhum PlayerHealth encontrado na cena.");
            return;
        }

        // inicializa os valores da barra
        slider.maxValue = playerHealth.maxHealth;
        slider.value = playerHealth.currentHealth;

        // assina o evento de mudança de vida
        playerHealth.OnHealthChanged += OnHealthChanged;
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= OnHealthChanged;
        }
    }

    void OnHealthChanged(int current, int max)
    {
        if (slider == null) return;

        slider.maxValue = max;
        slider.value = current;
    }
}
