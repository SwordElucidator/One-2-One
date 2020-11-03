using UnityEngine;
using UnityEngine.UI;

public class HomeMainController : MonoBehaviour
{
    // welcome
    public GameObject welcome;
    public Text welcomeGold;
    // public Button btn;

    // settings
    public GameObject settings;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Init()
    {
        CheckWelcome();
        // TODO money(unit) + gold
        // TODO score + champion
        // TODO actionValue
        // TODO 来点动画？
    }

    private void CheckWelcome()
    {
        const bool show = true; // TODO perGet
        welcome.SetActive(show);
        welcomeGold.text = "100"; // TODO value
    }

    public void HideWelcome()
    {
        welcome.SetActive(false);
        // btn.onClick.AddListener();
        // TODO perSet
    }

    public void SettingsPress()
    {
        settings.SetActive(true);
        // TODO nameValue
    }

    public void MoneyInfoPress()
    {
        // TODO nav
    }

    public void GoldInfoPress()
    {
        // 应该是没有跳转
    }

    public void GameSinglePress()
    {
        // TODO nav
    }

    public void MultiSinglePress()
    {
        // TODO nav
    }
}