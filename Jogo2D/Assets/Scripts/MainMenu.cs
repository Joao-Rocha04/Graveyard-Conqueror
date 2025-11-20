using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Painéis do Menu")]
    public GameObject menuRoot;            // MainMenuPanel
    public GameObject instructionsPanel;   // InstructionsPanel

    void Awake()
    {
        if (menuRoot == null)
            menuRoot = gameObject;

        // Começa com o menu visível e jogo pausado
        menuRoot.SetActive(true);
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // ------------ BOTÃO PLAY ------------
    public void OnPlayClicked()
    {
        menuRoot.SetActive(false);
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // ------------ BOTÃO INSTRUÇÕES ------------
    public void OnInstructionsClicked()
    {
        instructionsPanel.SetActive(true);
        menuRoot.SetActive(false);
    }

    // ------------ BOTÃO VOLTAR ------------    
    public void OnBackFromInstructions()
    {
        instructionsPanel.SetActive(false);
        menuRoot.SetActive(true);
    }

    // ------------ BOTÃO SAIR (OPCIONAL) ------------
    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
