using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{
    public static GameEndUI Instance { get; private set; }

    [Header("Painéis de fim de jogo")]
    public GameObject defeatPanel;
    public GameObject victoryPanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    // Chamado quando o player MORRE
    public void ShowDefeat()
    {
        Time.timeScale = 0f;
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null)  defeatPanel.SetActive(true);
    }

    // Chamado quando o player VENCE
    public void ShowVictory()
    {
        Time.timeScale = 0f;
        if (defeatPanel != null)  defeatPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    // Botão MENU (tanto vitória quanto game over usam isto)
    public void OnMenuClicked()
    {
        Time.timeScale = 1f;

        // como o menu inicial está na mesma cena,
        // basta recarregar a cena atual:
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        // ao recarregar, o MainMenu.Awake() roda,
        // timeScale volta para 0 e o menu inicial aparece.
    }
}
