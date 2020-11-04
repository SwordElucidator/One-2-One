using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class HomeMainController : MonoBehaviour, IPlayerMessageTarget // , IHomeMainMessageTarget
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

    private void Init()
    {
        CheckWelcomeVisible();
        RefreshMainView();
    }

    public void OnUserDataChange()
    {
        RefreshMainView();
    }

    /**
     ***************************************** Main ******************************************* 
     */
    private void RefreshMainView()
    {
        mainMoneyText.text = UserData.Instance().money.ToString(CultureInfo.CurrentCulture);
        mainCoinText.text = UserData.Instance().coin.ToString();
        mainScoreText.text = UserData.Instance().bestScore.ToString();
        mainChampionText.text = UserData.Instance().championCount.ToString();
        const bool needMonty = true; // TODO check
        mainMultiNeedText.text = "12"; // TODO check
        mainMultiNeedImageMoney.gameObject.SetActive(needMonty);
        mainMultiNeedImageCoin.gameObject.SetActive(!needMonty);
    }

    /**
     ***************************************** Welcome ******************************************* 
     */
    private void CheckWelcomeVisible()
    {
        // bool show = Utils.IsSameDay(DateTime.Now, UserData.Instance().lastLogin);
        bool show = true;
        welcomeObj.SetActive(show);
        welcomeGoldText.text = Config.daySignCoin.ToString();
    }

    public void OnWelcomeHidePress()
    {
        welcomeObj.SetActive(false);
        Player.OnWelcomeStart();
        Player.EventUserDataRefresh(gameObject);
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