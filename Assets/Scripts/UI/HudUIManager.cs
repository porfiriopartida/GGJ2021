using LopapaGames.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class HudUIManager : MonoBehaviour
{
    public GameObject UseButton;
    public GameObject ReportButton;
    public GameObject SabotageButton;
    public GameObject KillButton;
    public Image ProgressBar;
    public Float Progress;

    public GameObject[] ImpostorUIElements;
    void Start()
    {
        SetCanUse(false);
        SetCanReport(false);
        SetCanKill(false);
    }

    private bool IsAlive()
    {
        return SceneStateManager.Instance.IsAlive();
    }

    public void SetCanUse(bool canUse)
    {
        UseButton.SetActive(canUse);
    }
    public void SetCanReport(bool canUse)
    {
        ReportButton.SetActive(IsAlive() && canUse);
    }
    public void SetCanKill(bool canUse)
    {
        KillButton.SetActive(IsAlive() && canUse);
    }
    public void RefreshProgress()
    {
        ProgressBar.fillAmount = Progress.Value;
    }
}
