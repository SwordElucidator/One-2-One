using UnityEngine;
using UnityEngine.UI;

// public interface IHomeMainMessageTarget : IEventSystemHandler
// {
//     // 可通过消息系统调用的函数
//     void OnMoneyOrCoinChange(BaseEventData data);
//     void OnScoreOrChampionChange(BaseEventData data);
// }

// public class Datass : BaseEventData
// {
//     public Datass(EventSystem eventSystem) : base(eventSystem)
//     {
//     }
// }

public class HomeMainController : MonoBehaviour // , IHomeMainMessageTarget
{
    // main
    public Text mainMoneyText;
    public Text mainCoinText;
    public Text mainScoreText;
    public Text mainChampionText;
    public Text mainMultiNeedText;
    public Image mainMultiNeedImageMoney;
    public Image mainMultiNeedImageCoin;

    // welcome
    public GameObject welcomeObj;
    public Text welcomeGoldText;

    // settings
    public GameObject settingsObj;
    public RawImage settingsAvatarImage;
    public InputField settingsNameInput;

    // agreement
    public GameObject agreement; // TODO
    
    // bill
    public GameObject billObj;
    public ScrollRect billListScroll;
    public Text billSpeedQueueCountText;
    public GameObject billSpeedFinishObj;
    public Text billSpeedFinishTitleText;

    void Start()
    {
        Init();
    }

    void Update()
    {
    }

    // public static void OnMemoryChange(GameObject target,)
    // {
    //     ExecuteEvents.Execute<IHomeMainMessageTarget>(target, new Datass(EventSystem.current), (x,y)=>x.OnMoneyOrCoinChange(y));
    // }

    private void Init()
    {
        CheckWelcomeVisible();
        RefreshMainView();
    }

    /**
     ***************************************** Main ******************************************* 
     */
    private void RefreshMainView()
    {
        mainMoneyText.text = "99.99"; // TODO perGet
        mainCoinText.text = "2333"; // TODO petGet
        mainScoreText.text = "5600000"; // TODO petGet
        mainChampionText.text = "111"; // TODO petGet
        const bool needMonty = true; // TODO check
        mainMultiNeedText.text = "12"; // TODO check
        mainMultiNeedImageMoney.gameObject.SetActive(needMonty);
        mainMultiNeedImageCoin.gameObject.SetActive(!needMonty);
    }

    // public void OnMoneyOrCoinChange(BaseEventData data)
    // {
    // }
    //
    // public void OnScoreOrChampionChange(BaseEventData data)
    // {
    // }

    /**
     ***************************************** Welcome ******************************************* 
     */
    private void CheckWelcomeVisible()
    {
        const bool show = true; // TODO perGet
        welcomeObj.SetActive(show);
        welcomeGoldText.text = "100"; // TODO perGet
    }

    public void OnWelcomeHidePress()
    {
        welcomeObj.SetActive(false);
        // TODO perSet
    }

    /**
     ***************************************** Settings ******************************************* 
     */
    public void OnSettingsShowPress()
    {
        settingsObj.SetActive(true);
        // settingsAvatarImage.texture = ???; // TODO perGet
        settingsNameInput.text = "你猜"; // TODO perGet
    }

    public void OnSettingsHidePress()
    {
        settingsObj.SetActive(false);
    }

    public void OnSettingsAvatarPress()
    {
        // TODO selectAvatar
    }

    public void OnSettingsNameSavePress()
    {
        // Debug.Log("--->" + settingsNameInput.text);
        // TODO saveName
        OnSettingsHidePress();
    }

    /**
     ***************************************** Agreement ******************************************* 
     */
    public void OnAgreementShowPress()
    {
        agreement.SetActive(true);
    }

    public void OnAgreementHidePress()
    {
        agreement.SetActive(true);
    }

    /**
     ***************************************** Bill ******************************************* 
     */
    public void OnBillShowPress()
    {
        billObj.SetActive(true);
        billSpeedFinishObj.SetActive(false);
        // TODO init
    }

    public void OnBillHidePress()
    {
        billObj.SetActive(false);
    }

    public void BillCashOutOver()
    {
        // TODO refreshView
    }

    public void OnBillSpeedUpPress()
    {
        // TODO ad
    }
    
    public void BillSpeedUpFinish()
    {
        // TODO finishAlert
    }
    
    public void OnBillSpeedUpFinishHidePress()
    {
        billSpeedFinishObj.SetActive(false);
    }

    /**
     ***************************************** CashOut ******************************************* 
     */
    public void OnCashOutShowPress()
    {
        // TODO nav
    }

    public void OnCashOutHidePress()
    {
        // TODO nav
    }

    public void OnCashOutActionPress()
    {
        // TODO nav
    }

    /**
     ***************************************** Single ******************************************* 
     */
    public void OnGameSinglePress()
    {
        // TODO nav
    }

    /**
     ***************************************** Multi ******************************************* 
     */
    public void OnGameMultiPress()
    {
        // TODO check + alert + nav
    }
}