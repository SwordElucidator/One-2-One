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
    public Button billCashOutBtn;
    public Button billSpeedUpBtn;
    public Text billSpeedQueuePeopleText;
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
        bool needMonty = UserData.Instance().coin < Config.MultiNeedCoin;
        mainMultiNeedText.text = (needMonty ? Config.MultiNeedMoney : Config.MultiNeedCoin).ToString();
        mainMultiNeedImageMoney.gameObject.SetActive(needMonty);
        mainMultiNeedImageCoin.gameObject.SetActive(!needMonty);
    }

    /**
     ***************************************** Welcome ******************************************* 
     */
    private void CheckWelcomeVisible()
    {
        bool show = Utils.IsSameDay(DateTime.Now, UserData.Instance().lastLogin); // true;
        welcomeObj.SetActive(show);
        welcomeGoldText.text = Config.DaySignCoin.ToString();
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
        agreement.SetActive(false);
    }

    /**
     ***************************************** Bill ******************************************* 
     */
    public void OnBillShowPress()
    {
        billObj.SetActive(true);
        billSpeedFinishObj.SetActive(false);
        // bills
        List<Bill> bills = UserData.Instance().bills;
        foreach (var b in bills)
        {
            // Debug.Log("bill[" + i + "]--->" + JsonUtility.ToJson(b));
            var billsItem = Instantiate(billsItemObj, billsScrollContentTransform);
            // transform(content加了VerticalLayout和fitter就没必要了，还能自动算高度)
            // billsItem.transform.SetParent(billsScrollContentTransform);
            // var reactTransform = billsItem.GetComponent<RectTransform>();
            // var rect = reactTransform.rect;
            // billsItem.transform.position -= Vector3.up * (rect.height - rect.position.y) * i;
            // data
            var dateTime = Utils.GetDateTime(b.data);
            billsItem.transform.Find("Date").GetComponent<Text>().text = dateTime.Month + "/" + dateTime.Day;
            var changeColor = new Color(0.81f, 0f, 0.1f, 1f);
            var changeText = b.change.ToString(CultureInfo.InvariantCulture);
            if (b.change >= 0)
            {
                changeColor = new Color(0.29f, 0.56f, 0.88f, 1f);
                changeText = "+" + changeText;
            }
            billsItem.transform.Find("Change").GetComponent<Text>().color = changeColor;
            billsItem.transform.Find("Change").GetComponent<Text>().text = changeText;
            billsItem.transform.Find("Unit").GetComponent<Text>().text = b.unit;
        }
        RefreshBillStatus();
    }

    private void RefreshBillStatus()
    {
        var set = UserData.Instance().cashOutSet;
        billCashOutBtn.gameObject.SetActive(!set);
        billSpeedUpBtn.gameObject.SetActive(set);
        if (set)
        {
            billSpeedQueuePeopleText.text = "12232141"; // TODO 排队人数
        }
    }

    public void OnBillHidePress()
    {
        billObj.SetActive(false);
    }

    public void BillCashOutOver()
    {
        // TODO cashOut页面跳回来之后 refreshView
        RefreshBillStatus();
    }

    public void OnBillSpeedUpPress()
    {
        // TODO ad
    }

    public void BillSpeedUpFinish()
    {
        billSpeedFinishObj.SetActive(true);
        billSpeedFinishTitleText.text =
            "You have surpassed  <color=\"#F8C11C\">" + 2312321 + // TODO
            "</color> people in line Keep on logging and earning! Cash out at the same time!";
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
        // TODO check + alert(style修改) + nav
    }
}