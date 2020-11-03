using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.MoPub;


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
        // Initialize the MoPub SDK.
#if UNITY_ANDROID
        MoPub.Initialize("6cfbf16b2ab046b58b9573ee008494d8");
#elif UNITY_IPHONE
        MoPub.Initialize("19743bae2b8440d2b7a891be3fb82692");
#else
        MoPub.Initialize("19743bae2b8440d2b7a891be3fb82692");
#endif
        
        
    }

}