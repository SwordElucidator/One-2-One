using System;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    private static UserData _instance;

    public static UserData Instance()
    {
        if (_instance != null) return _instance;
        _instance = Get();
        return _instance;
    }

    public float money = 0;
    public int coin = 0;
    public int bestScore = 0;
    public int championCount = 0;
    public long lastLogin = 0;
    public String userName = "";
    public bool cashOutSet = false;
    // public bool soundEnable = true;
    public List<Bill> bills = new List<Bill>();

    private const String KeyUserData = "1+1_user_data";
    private const String KeyUserDataBill = "1+1_user_data_bill";

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
        // Debug.Log("Get-UserData--->" + json);
        String bill = PlayerPrefs.GetString(KeyUserDataBill);
        List<Bill> bills = Utils.JsonToList<Bill>(bill);
        // Debug.Log("Get-Bills--->" + Utils.ListToJson(bills));
        UserData userData  = string.IsNullOrEmpty(json) ? new UserData() : JsonUtility.FromJson<UserData>(json);
        userData.bills = bills;
        return userData;
    }

    public void Save()
    {
        String json = JsonUtility.ToJson(this);
        // Debug.Log("Set-UserData--->" + json);
        String bill = Utils.ListToJson(bills);
        // Debug.Log("Set-Bills--->" + bill);
        PlayerPrefs.SetString(KeyUserData, json);
        PlayerPrefs.SetString(KeyUserDataBill, bill);
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