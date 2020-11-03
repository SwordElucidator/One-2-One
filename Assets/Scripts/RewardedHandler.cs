using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardedHandler : MonoBehaviour
{
    private RewardedAd _rewardedAd;
    
    [CanBeNull] public EventTrigger.TriggerEvent adLoaded;
    [CanBeNull] public EventTrigger.TriggerEvent rewarded;
    [CanBeNull] public EventTrigger.TriggerEvent closed;
    public bool autoRecreate = false;
    
    
#if UNITY_ANDROID
    private const string RewardAdUnitId = "ca-app-pub-2716680030920038/8203292065";
#elif UNITY_IPHONE
    private const string RewardAdUnitId = "ca-app-pub-2716680030920038/7784489667";
#else
    private const string RewardAdUnitId = "";
#endif
    
    
    // Start is called before the first frame update
    void Start()
    {
        CreateAd();
    }

    public void CreateAd()
    {
        _rewardedAd = new RewardedAd(RewardAdUnitId);
        
        // Called when an ad request has successfully loaded.
        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        _rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        var request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        _rewardedAd.LoadAd(request);
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
        var data = new BaseEventData(EventSystem.current);
        adLoaded?.Invoke(data);
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToLoad event received with message: "
            + args.Message);
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
            + args.Message);
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
        var data = new BaseEventData(EventSystem.current);
        closed?.Invoke(data);
        if (autoRecreate)
        {
            CreateAd();
        }
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
            + amount + " " + type);
        var data = new BaseEventData(EventSystem.current);
        rewarded?.Invoke(data);
    }

    public void WatchAd()
    {
        if (_rewardedAd.IsLoaded()) {
            _rewardedAd.Show();
        }
    }
}
