using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Canvas que contém a UI do menu inicial (botão Jogar, título, etc.)")]
    public GameObject menuCanvas;

    [Tooltip("Raiz de todos os objetos de gameplay (player, inimigos, HUD, etc.)")]
    public GameObject gameplayRoot;

    void Start()
    {
        // Garantias básicas para não dar null reference
        if (menuCanvas != null)
            menuCanvas.SetActive(true);

        if (gameplayRoot != null)
            gameplayRoot.SetActive(false);

        Time.timeScale = 0f; // jogo pausado enquanto o menu está aberto
    }

    // Chamado pelo botão "Jogar"
    public void PlayGame()
    {
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        if (gameplayRoot != null)
            gameplayRoot.SetActive(true);

        Time.timeScale = 1f; // jogo roda normalmente
    }
}
