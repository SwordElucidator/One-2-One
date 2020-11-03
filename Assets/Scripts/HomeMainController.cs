using UnityEngine;
using UnityEngine.UI;

public class HomeMainController : MonoBehaviour
{
    // main

    // welcome
    public GameObject welcome;

    public Text welcomeGold;
    // public Button btn;

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
        // TODO money(unit) + gold
        // TODO score + champion
        // TODO actionValue
        // TODO 来点动画？
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
    public void OnMultiSinglePress()
    {
        // TODO check + alert + nav
    }
}