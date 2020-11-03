using System;
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

    private void OnGUI()
    {
    }
}