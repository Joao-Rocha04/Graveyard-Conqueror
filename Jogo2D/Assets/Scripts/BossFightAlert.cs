using UnityEngine;
using TMPro;

public class BossFightAlert : MonoBehaviour
{
    [Header("Referências")]
    public CanvasGroup canvasGroup;
    public TMP_Text messageText;

    [Header("Configuração")]
    public string message = "FIGHT WITH BOSS";
    public float blinkSpeed = 4f;  // velocidade da piscada

    private bool isActive;

    void Awake()
    {
        if (messageText != null)
            messageText.text = message;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;   // começa invisível

        isActive = false;
    }

    void Update()
    {
        if (!isActive || canvasGroup == null) return;

        // Piscando a tela (alpha do CanvasGroup)
        float t = Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1f);
        canvasGroup.alpha = Mathf.Lerp(0.3f, 1f, t);
    }

    public void ShowAlert()
    {
        isActive = true;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    public void HideAlert()
    {
        isActive = false;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}
