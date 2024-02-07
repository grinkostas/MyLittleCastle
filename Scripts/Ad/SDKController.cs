using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using HomaGames.HomaBelly;
using NepixSignals;

public static class SDKController
{
    public static TheSignal Initialized { get; } = new();
    

    public static void SendEvent(string eventValue)
    {
        DefaultAnalytics.LevelStarted(eventValue);
        DefaultAnalytics.LevelCompleted();
        Debug.Log("New Design Event " + eventValue);
        //GameAnalytics.NewDesignEvent(eventValue);
    }

    public static void LevelStart(string eventValue)
    {
        DefaultAnalytics.LevelStarted(eventValue);
        Debug.Log("Level Started " + eventValue);
    }

    public static void LevelComplete()
    {
        DefaultAnalytics.LevelCompleted();
        Debug.Log("Level Completed ");
    }
    /*
    public static void Initialize()
    {
        return;
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            Initialized.Dispatch();
        };

        MaxSdk.InitializeSdk();
    }
    */
    
}
