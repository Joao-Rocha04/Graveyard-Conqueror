using UnityEngine;
using TMPro;
using System.Collections;

public class BossFightAlert : MonoBehaviour
{
    [Header("Referências")]
    public CanvasGroup canvasGroup;   // CanvasGroup do painel (BossFightPanel)
    public TMP_Text messageText;      // Texto "FIGHT WITH BOSS"

    [Header("Configuração")]
    public string message = "FIGHT WITH BOSS";
    public float blinkSpeed = 4f;     // velocidade da piscada
    public float blinkDuration = 5f;  // quanto tempo piscando (em segundos)
    public bool pauseGame = true;     // se deve pausar o jogo

    private bool isActive;
    private Coroutine blinkRoutine;
    private float previousTimeScale = 1f;

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

        // piscando usando tempo NÃO escalado (funciona mesmo com timeScale = 0)
        float t = Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1f);
        canvasGroup.alpha = Mathf.Lerp(0.3f, 1f, t);
    }

    /// <summary>
    /// Mostra o alerta, pausa o jogo e faz piscar por X segundos.
    /// Depois some e o jogo volta.
    /// </summary>
    public void ShowAlertAndPause()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        isActive = true;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        if (pauseGame)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;  // PAUSA O JOGO
        }

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkForSeconds());
    }

    private IEnumerator BlinkForSeconds()
    {
        float endTime = Time.unscaledTime + blinkDuration;

        while (Time.unscaledTime < endTime)
        {
            // espera frame a frame; o efeito de piscar está no Update
            yield return null;
        }

        // Depois de X segundos: some o alerta e volta o jogo
        HideAlertAndResume();
    }

    /// <summary>
    /// Some com o alerta e volta o jogo ao timeScale original.
    /// </summary>
    public void HideAlertAndResume()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }

        isActive = false;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (pauseGame)
            Time.timeScale = previousTimeScale; // JOGO VOLTA

        // opcional: esconder o painel de vez
        // gameObject.SetActive(false);
    }
}
