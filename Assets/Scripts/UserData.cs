using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class UserData
{
    private static UserData _instance;

    public static UserData Instance()
    {
        if (_instance != null) return _instance;
        _instance = Get();
        return _instance;
    }

    public float money = 0; // TODO set
    public int coin = 0; // TODO set
    public int bestScore = 0; // TODO set
    public int championCount = 0; // TODO set
    public long lastLogin = 0;
    public String userName = "";

    public bool cashOutSet = false;

    // public bool soundEnable = true;
    public List<Bill> bills = new List<Bill>();
    public CashOutWaiting waiting;

    private const String KeyUserData = "1+1_user_data";
    private const String KeyUserDataBill = "1+1_user_data_bill";
    private const String KeyUserDataWaiting = "1+1_user_data_waiting";

    // public UserData(int money, int coin, int bestScore, int championCount, long lastLogin, string userName, bool cashOutSet, bool soundEnable)
    // {
    //     this.money = money;
    //     this.coin = coin;
    //     this.bestScore = bestScore;
    //     this.championCount = championCount;
    //     this.lastLogin = lastLogin;
    //     this.userName = userName;
    //     this.cashOutSet = cashOutSet;
    //     this.soundEnable = soundEnable;
    // }

    private static UserData Get()
    {
        String json = PlayerPrefs.GetString(KeyUserData);
        UserData userData = string.IsNullOrEmpty(json) ? new UserData() : JsonUtility.FromJson<UserData>(json);
        // Debug.Log("Get-UserData--->" + json);
        String billsJson = PlayerPrefs.GetString(KeyUserDataBill);
        List<Bill> billsObj = string.IsNullOrEmpty(billsJson) ? new List<Bill>() : Utils.JsonToList<Bill>(billsJson);
        userData.bills = billsObj;
        // Debug.Log("Get-Bills--->" + Utils.ListToJson(bills));
        String waitingJson = PlayerPrefs.GetString(KeyUserDataWaiting);
        CashOutWaiting waitingObj = string.IsNullOrEmpty(waitingJson)
            ? new CashOutWaiting()
            : JsonUtility.FromJson<CashOutWaiting>(waitingJson);
        userData.waiting = waitingObj;
        userData.waiting.Init();
        // Debug.Log("Get-waiting--->" + waitingJson);
        return userData;
    }

    public void Save()
    {
        String json = JsonUtility.ToJson(this);
        // Debug.Log("Set-UserData--->" + json);
        String billsJson = Utils.ListToJson(bills);
        // Debug.Log("Set-Bills--->" + billsJson);
        String waitingJson = Utils.ListToJson(waiting);
        // Debug.Log("Set-waiting--->" + waitingJson);
        PlayerPrefs.SetString(KeyUserData, json);
        PlayerPrefs.SetString(KeyUserDataBill, billsJson);
        PlayerPrefs.SetString(KeyUserDataWaiting, waitingJson);
    }
}

public class Bill
{
    public long data;
    public float change;
    public String unit;

    public static Bill Create(long pData, float pChange, String pUnit)
    {
        return new Bill {data = pData, change = pChange, unit = pUnit};
    }
}

public class CashOutWaiting
{
    private static readonly int[] WaitingNum = {270000, 330000};
    private static readonly int WaitingNumMax = 8;
    private static readonly float SpeedUpConstant = 17.2f;
    private static readonly int SlowDownSince = 10000;
    private static readonly float SlowDownConstant = 12.4f;
    private static readonly int SingleSlowPeriod = 60 * 1000;

    public int waitingNum;
    public long lastSpeedUpTime;
    private long _lastUpdateTime;

    public CashOutWaiting()
    {
        Init();
    }

    public void Init()
    {
        if (_lastUpdateTime == 0)
        {
            waitingNum = _getWaitingNum();
            _lastUpdateTime = Utils.GetTimeStamp(DateTime.Now);
            lastSpeedUpTime = Utils.GetTimeStamp(new DateTime(2020, 11, 1, 0, 0, 0, 0));
        }
        else
        {
            var results = _slowDown(waitingNum, Utils.GetTimeStamp(DateTime.Now) - _lastUpdateTime);
            _lastUpdateTime = (results[0] == waitingNum) ? _lastUpdateTime : Utils.GetTimeStamp(DateTime.Now);
            waitingNum = results[0];
        }
    }

    private int _getWaitingNum()
    {
        return (int) ((WaitingNum[1] - WaitingNum[0]) * new Random().NextDouble() + WaitingNum[0]);
    }

    private int[] _slowDown(int currentWaiting, long passedTime)
    {
        var slowDown = 0;
        var currentWaitingNum = currentWaiting;
        if (passedTime >= SingleSlowPeriod)
        {
            // 至少过去了60秒
            // 处理下滑
            // 当数量大时，不但不下滑，还下降
            // 当数量小时，反弹
            // 10000 (20, 1200000)
            // - log10(20 / 10000) = 2.6989;
            // - log10(1200000 / 10000) = - 2.0792;
            for (var i = 0; i < passedTime / SingleSlowPeriod; i++)
            {
                var s = -Math.Floor(Math.Log10(currentWaitingNum / SlowDownSince) * SlowDownConstant *
                                    (new Random().NextDouble() * 0.3 + 0.85));
                slowDown += (int) s;
                currentWaitingNum = currentWaitingNum + (int) s;
            }
        }

        return new[] {currentWaitingNum, slowDown};
    }

    public int SpeedUp(int waitingCount)
    {
        var toSpeedUp = Math.Ceiling(Math.Pow(SpeedUpConstant, Math.Log10(waitingCount) - 2));
        if (waitingCount - toSpeedUp <= WaitingNumMax)
        {
            toSpeedUp = waitingCount - WaitingNumMax;
        }

        return (int) toSpeedUp;
    }
}