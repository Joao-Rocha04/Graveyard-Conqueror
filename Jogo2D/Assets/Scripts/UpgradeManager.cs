using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;     
    public Button btnPlayerSpeed;
    public Button btnEnemySlow;

    bool open;

    void Start()
{
    if (panel) panel.SetActive(false);

    if (btnPlayerSpeed)
    {
        btnPlayerSpeed.onClick.RemoveAllListeners();
        btnPlayerSpeed.onClick.AddListener(() => { 
            Debug.Log("Click PlayerSpeed"); 
            Pick_PlayerSpeed(); 
        });
    }

    if (btnEnemySlow)
    {
        btnEnemySlow.onClick.RemoveAllListeners();
        btnEnemySlow.onClick.AddListener(() => { 
            Debug.Log("Click EnemySlow"); 
            Pick_EnemySlow(); 
        });
    }
}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) Toggle();
    }

    public void ShowOnLevelUp() { Open(); }

    public void Toggle() { if (open) Close(); else Open(); }

    void Open()
    {
        open = true;
        panel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    void Close()
    {
        open = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    void AfterPick() => Close();

    public void Pick_PlayerSpeed()
    {
        GameUpgrades.Instance.Upgrade_PlayerSpeed();
        AfterPick();
    }

    public void Pick_EnemySlow()
    {
        GameUpgrades.Instance.Upgrade_EnemySlow();
        AfterPick();
    }
}
