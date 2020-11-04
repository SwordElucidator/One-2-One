using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.MoPub;
using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("第一个场景加载前1");
    }

    private void Awake()
    {
        Debug.Log("Awake");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod()
    {
        Debug.Log("第一个场景加载后2");
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("第一个场景加载后3");
    }

    private void Reset()
    {
        Debug.Log("Reset");
    }

    private void Start()
    {
        Debug.Log("Start");
        
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

    private void FixedUpdate()
    {
        // Debug.Log("FixedUpdate");
    }

    private void Update()
    {
        // Debug.Log("Update");
    }

    private void LateUpdate()
    {
        // Debug.Log("LateUpdate");
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("OnApplicationFocus");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("OnApplicationPause");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
    }

}