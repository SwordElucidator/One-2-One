using System;
using System.Collections.Generic;
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
    public Transform billsScrollContentTransform;
    public GameObject billsItemObj;
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
        bool needMonty = UserData.Instance().coin < Config.multiNeedCoin;
        mainMultiNeedText.text = (needMonty ? Config.multiNeedMoney : Config.multiNeedCoin).ToString();
        mainMultiNeedImageMoney.gameObject.SetActive(needMonty);
        mainMultiNeedImageCoin.gameObject.SetActive(!needMonty);
        
        // TODO test
        // Bill b1 = Bill.Create(Utils.GetTimeStamp(DateTime.Now), 10, Config.GetUnit());
        // UserData.Instance().bills.Add(b1);
        // Bill b2 = Bill.Create(Utils.GetTimeStamp(DateTime.Now), -10, Config.GetUnit());
        // UserData.Instance().bills.Add(b2);
        // Bill b3 = Bill.Create(Utils.GetTimeStamp(DateTime.Now), 1000, Config.GetUnit());
        // UserData.Instance().bills.Add(b3);
        // UserData.Instance().Save();
    }

    /**
     ***************************************** Welcome ******************************************* 
     */
    private void CheckWelcomeVisible()
    {
        bool show = Utils.IsSameDay(DateTime.Now, UserData.Instance().lastLogin); // true;
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
        settingsNameInput.text = UserData.Instance().userName;
    }

    public void OnSettingsHidePress()
    {
        settingsObj.SetActive(false);
    }

    public void OnSettingsAvatarPress()
    {
        Player.SelectAvatar();
    }

    public void OnSettingsNameSavePress()
    {
        Player.SaveUserName(settingsNameInput.text);
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
        List<Bill> bills = UserData.Instance().bills;
        for (int i = 0; i < bills.Count; i++)
        {
            Bill b = bills[i];
            Debug.Log("bill[" + i + "]--->" + JsonUtility.ToJson(b));
            // Text[] texts = billsItemObj.GetComponents<Text>();
            // texts[0].text = b.data.ToString();
            // texts[1].text = b.change.ToString(CultureInfo.InvariantCulture);
            // texts[2].text = b.unit;
            Instantiate(billsItemObj, billsScrollContentTransform);
        }
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