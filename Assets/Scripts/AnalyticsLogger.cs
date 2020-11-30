using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsLogger : MonoBehaviour
{
    static void LogSingleGameOver(float hardness, int progress, float money, bool reviveUsed)
    {
        Firebase.Analytics.Parameter[] gameOverParameters = {
            new Firebase.Analytics.Parameter("hardness", hardness),
            new Firebase.Analytics.Parameter("progress", progress),
            new Firebase.Analytics.Parameter("money", money),
            new Firebase.Analytics.Parameter("reviveUsed", reviveUsed.ToString()),
        };
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("gameOverSingle", gameOverParameters);
    }
    
    static void LogMultiGameOver(float hardness, int progress, float money, int rank)
    {
        Firebase.Analytics.Parameter[] gameOverParameters = {
            new Firebase.Analytics.Parameter("hardness", hardness),
            new Firebase.Analytics.Parameter("progress", progress),
            new Firebase.Analytics.Parameter("money", money),
            new Firebase.Analytics.Parameter("rank", rank),
        };
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("gameOverMulti", gameOverParameters);
    }

    static void LogGainMoney(float fromMoney, float money)
    {
        if ((int) Math.Floor(fromMoney / 10) != (int) Math.Floor((fromMoney + money) / 10))
        {
            // 每10元算一次
            Firebase.Analytics.FirebaseAnalytics
                .LogEvent("GainMoney" + (int) Math.Floor(fromMoney / 10));
        }
    }
    
    static void LogWatchReward()
    {
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("watchReward");
    }
    
    static void LogRewarded()
    {
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("rewarded");
    }
}
