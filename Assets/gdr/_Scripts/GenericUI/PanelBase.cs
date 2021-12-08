using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour
{
    public Button closePanelButton;
    public Animator panelAnimator;

    public bool panelIsOpen;

    private void Start()
    {
        ButtonsSetup();
    }

    private void Update()
    {
        //if (panelIsOpen)
        //    rewardButton.interactable = ShowAds.Instance.RewardAdsIsReady();
    }

    private void ButtonsSetup()
    {
        if (closePanelButton != null)
        {
            closePanelButton.onClick.RemoveAllListeners();
            closePanelButton.onClick.AddListener(() => { ClosePanel(); });
        }
    }

    public void OpenPanel([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        GenericUI.PlayAnim(panelAnimator, "IsOpen");
        panelIsOpen = true;
        // UIManager.PanelIsOpen = true; UIManager.Instance.SetCurrentPanel(gameObject);
    }

    public void GetReward()
    {
        //if (rewardType == RewardType.NextWeek)
        //{
        //    //Debug.Log($"LEVEL = {GameManager.Instance.currentPerson.week}");
        //    AppMetrica.Instance.ReportEvent($"level_start", MainData.GetStandartParameters());
        //    AppMetrica.Instance.ReportEvent("offer_energy_for_ad_next_level_skip", MainData.GetStandartParameters());
        //}

        //MainData.Instance.GetReward(rewardType, 1);
        ClosePanel();
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }

    public void GetDoubleReward()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        //UnityAction callback = () =>
        //{
        //    MainData.Instance.GetReward(rewardType, 3);
        //    ClosePanel();
        //};
        //ShowAds.Instance.ShowRewardAd((rewardType == RewardType.NextWeek ? BonusTypeEnum.next_week : BonusTypeEnum.offline_bonus), callback);
    }

    public void ClosePanel()
    {
        GenericUI.StopAnim(panelAnimator, "IsOpen");
        panelIsOpen = false;
        // UIManager.PanelIsOpen = false;
    }
}
