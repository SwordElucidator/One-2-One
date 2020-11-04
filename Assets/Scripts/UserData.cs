using System;
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
    public bool soundEnable = true;
    public Bill[] bills;

    private const String KeyUserData = "1+1_user_data";

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
        return string.IsNullOrEmpty(json) ? new UserData() : JsonUtility.FromJson<UserData>(json);
    }

    public void Save()
    {
        String json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(KeyUserData, json);
    }
}

public class Bill
{
    public long data;
    public int change;
    public String unit;

    public static Bill Create(long pData, int pChange, String pUnit)
    {
        return new Bill {data = pData, change = pChange, unit = pUnit};
    }
}