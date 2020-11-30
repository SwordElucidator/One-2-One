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
        UserData.Instance().coin += Config.DaySignCoin;
        UserData.Instance().lastLogin = Utils.GetTimeStamp(DateTime.Now);
        UserData.Instance().Save();
        AddBill(Config.DaySignCoin);
    }

    public static void SaveUserName(String name)
    {
        UserData.Instance().userName = name;
        UserData.Instance().Save();
    }

    public static void SelectAvatar()
    {
        // TODO 选择头像
    }

    // TODO set
    public static void AddBill(float change)
    {
        Bill b = Bill.Create(Utils.GetTimeStamp(DateTime.Now), change, Config.GetUnit());
        UserData.Instance().bills.Add(b);
        UserData.Instance().Save();
    }
    
    public static void SetCashOut()
    {
        UserData.Instance().cashOutSet = true;
        UserData.Instance().Save();
    }

    public static bool CanSpeedUp()
    {
        var waiting = UserData.Instance().waiting;
        var last = Utils.GetDateTime(waiting.lastSpeedUpTime);
        var now = DateTime.Now;
        var sameHour = (last.Year == now.Year) && (last.DayOfYear == now.DayOfYear) && (last.Hour == now.Hour);
        return !sameHour && (waiting.SpeedUp(waiting.waitingNum) >= 0);
    }

    public static int SpeedUp()
    {
        var waiting = UserData.Instance().waiting;
        var toSpeedUp = waiting.SpeedUp(waiting.waitingNum);
        waiting.waitingNum -= toSpeedUp;
        waiting.lastSpeedUpTime = Utils.GetTimeStamp(DateTime.Now);
        UserData.Instance().Save();
        return toSpeedUp;
    }
}

public interface IPlayerMessageTarget : IEventSystemHandler
{
    void OnUserDataChange();
}