using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Configuração do Tempo")]
    public float totalTimeSeconds = 300f;      // 5 minutos (300s)

    [Header("Referências")]
    public TMP_Text timerText;                 // texto do timer
    public BossFightAlert bossFightAlert;      // alerta "FIGHT WITH BOSS"

    [Header("Aviso Final")]
    public float warningTimeSeconds = 30f;     // começa a piscar quando faltar isso
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float blinkSpeed = 8f;

    private float currentTime;
    private bool isWarning;
    private bool isFinished;

    void Start()
    {
        currentTime = totalTimeSeconds;

        if (timerText != null)
        {
            timerText.color = normalColor;
            UpdateTimerUI();
        }
    }

    void Update()
    {
        if (isFinished) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isFinished = true;
            UpdateTimerUI();

            // Quando o tempo acaba, dispara o alerta de BOSS
            if (bossFightAlert != null)
                bossFightAlert.ShowAlert();

            return;
        }

        if (!isWarning && currentTime <= warningTimeSeconds)
            isWarning = true;

        UpdateTimerUI();
        UpdateWarningEffect();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void UpdateWarningEffect()
    {
        if (timerText == null) return;

        if (isWarning && !isFinished)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color c = warningColor;
            c.a = Mathf.Lerp(0.3f, 1f, t);
            timerText.color = c;
        }
        else
        {
            timerText.color = normalColor;
        }
    }

    public bool IsFinished => isFinished;
}
