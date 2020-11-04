using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Player
{
    public static void EventUserDataRefresh(GameObject target)
    {
        ExecuteEvents.Execute<IPlayerMessageTarget>(target, null, (x,y)=>x.OnUserDataChange());
    }
    
    public static void OnWelcomeStart()
    {
        UserData.Instance().coin += Config.daySignCoin;
        UserData.Instance().lastLogin = Utils.GetTimeStamp(DateTime.Now);
        UserData.Instance().Save();
    }

    public static void SaveUserName()
    {
        // TODO
    }
    
    
}

public interface IPlayerMessageTarget : IEventSystemHandler
{
    void OnUserDataChange();
}