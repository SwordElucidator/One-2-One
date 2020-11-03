using UnityEngine;
using UnityEngine.UI;

public class HomeMainController : MonoBehaviour
{
    // main
    // public GameObject main;
    // public Button mainSettingsBtn;
    public Text mainMoneyText;
    public Text mainCoinText;
    public Text mainScoreText;
    public Text mainChampionText;
    // public Button mainSingleBtn;
    // public Button mainMultiBtn;
    public Text mainMultiNeedText;
    public Image mainMultiNeedImageMoney;
    public Image mainMultiNeedImageCoin;

    // welcome
    public GameObject welcome;
    public Text welcomeGold;

    // settings
    public GameObject settings;

    // agreement
    public GameObject agreement;

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
        RefreshPlayValue();
        // TODO actionValue
        // TODO 来点动画？
    }

    private void RefreshPlayValue()
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

    /**
     ***************************************** Welcome ******************************************* 
     */
    private void CheckWelcomeVisible()
    {
        const bool show = true; // TODO perGet
        welcome.SetActive(show);
        welcomeGold.text = "100"; // TODO value
    }

    public void OnWelcomeHidePress()
    {
        welcome.SetActive(false);
        // btn.onClick.AddListener();
        // TODO perSet
    }

    /**
     ***************************************** Settings ******************************************* 
     */
    public void OnSettingsShowPress()
    {
        settings.SetActive(true);
        // TODO nameValue
    }

    public void OnSettingsHidePress()
    {
        settings.SetActive(false);
    }

    public void OnSettingsAvatarPress()
    {
        // TODO selectAvatar
    }

    public void OnSettingsNameSavePress()
    {
        // TODO saveName
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
        // TODO nav
    }

    public void OnBillHidePress()
    {
        // TODO nav
    }

    public void OnBillCashOutPress()
    {
        // TODO nav
    }

    public void BillCashOutOver()
    {
        // TODO refreshView
    }

    public void OnBillSpeedUpPress()
    {
        // TODO ad + finishAlert
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