using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Player
{
    public static void EventUserDataRefresh(GameObject target)
    {
        ExecuteEvents.Execute<IPlayerMessageTarget>(target, null, (x, y) => x.OnUserDataChange());
    }

    public static void OnWelcomeStart()
    {
        UserData.Instance().coin += Config.daySignCoin;
        UserData.Instance().lastLogin = Utils.GetTimeStamp(DateTime.Now);
        UserData.Instance().Save();
        AddBill(Config.daySignCoin);
    }

    public static void SaveUserName(String name)
    {
        UserData.Instance().userName = name;
        UserData.Instance().Save();
    }

    public static void SelectAvatar()
    {
        // TODO 选择投降
    }

    public static void AddBill(float change)
    {
        Bill b = Bill.Create(Utils.GetTimeStamp(DateTime.Now), change, Config.GetUnit());
        UserData.Instance().bills.Add(b);
        UserData.Instance().Save();
    }
}

public interface IPlayerMessageTarget : IEventSystemHandler
{
    void OnUserDataChange();
}