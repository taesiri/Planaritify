using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;

public class AdAnalyticsObject : MonoBehaviour
{
    private static AdAnalyticsObject _instance;
    public static AdAnalyticsObject GetInstance => _instance;

    private void Awake()
    {
        if (GetInstance != null && GetInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowVideoAd()
    {
        var unityStats = Advertisement.IsReady("rewardedVideo");

        if (unityStats)
        {
            ShootUnityAdsVideo();
        }
    }

    public void ShootUnityAdsVideo()
    {
        if (Advertisement.IsReady("video"))
        {
            LogVideoAdShowed("Unity");
            var options = new ShowOptions {resultCallback = OnUnityAdsVideoResult};
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void OnUnityAdsVideoResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    // Analytics Events

    public void Start()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("AnalyticsObjectInitialized", "Initialized", 1);
    }

    public void LogLevelStart(string levelName)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelStarted", "Level", levelName);

        Analytics.CustomEvent("LevelStarted", new Dictionary<string, object>
        {
            {"Level", levelName}
        });
    }

    public void LogLevelRestarted(string levelName)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelRestarted", "Level", levelName);

        Analytics.CustomEvent("LevelRestarted", new Dictionary<string, object>
        {
            {"Level", levelName}
        });
    }

    public void LogLevelCompleted(string levelName, float totalTime)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelCompleted", "Level", levelName);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("TimeStats", levelName, totalTime);

        Analytics.CustomEvent("LevelCompleted", new Dictionary<string, object>
        {
            {"Level", levelName},
            {"Time", totalTime}
        });
    }

    public void LogVideoAdShowed(string provider)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("AdDisplayed", "Provider", provider);

        Analytics.CustomEvent("AdDisplayed", new Dictionary<string, object>
        {
            {"Provider", provider}
        });
    }

    public void LogLevelSelectionScreen()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelSelectionScreen", "Landed", "YES");
    }

    public void LogGameStarted()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("GameStarted", "IntroSceen", 1);
    }
}