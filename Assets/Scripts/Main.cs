using UnityEngine;
using GoogleMobileAds.Api;


public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
    }
    
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
    }

}