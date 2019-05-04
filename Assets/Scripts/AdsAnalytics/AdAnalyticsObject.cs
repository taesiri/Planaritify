using System.Collections.Generic;
using TapsellSDK;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AdAnalyticsObject : MonoBehaviour
{
    private static AdAnalyticsObject _instance;
    public static AdAnalyticsObject GetInstance => _instance;
    private TapsellAd _tapSellVideoAd;
    private TapsellAd _tapsellBanner;

    private void Awake()
    {
        if (GetInstance != null && GetInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        InvokeRepeating(nameof(ShootBannerAd), 60 , 180);
    }

    public void ShowVideoAd()
    {
        var unityStats = Advertisement.IsReady("rewardedVideo");

        if (!unityStats && _tapSellVideoAd == null)
        {
            QueryTapsellVideoAd();
        }
        else
        {
            if (unityStats)
            {
                var dice = Random.Range(0, 100);
                Debug.Log("Dice Value:" + dice);
                if (dice < 50)
                    ShootUnityAdsVideo();
                else
                    ShootTapsellVideo();
            }
            else
            {
                ShootTapsellVideo();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void PreloadAds()
    {
        if (_tapSellVideoAd == null)
        {
            QueryTapsellVideoAd();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PreloadAds();
    }

    private void QueryTapsellVideoAd()
    {
        Tapsell.requestAd("5ccddbb75b60f3000104b7e4", false,
            (TapsellAd result) =>
            {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                _tapSellVideoAd = result; // store this to show the ad later
            },

            (string zoneId) =>
            {
                // onNoAdAvailable
                Debug.Log("No Ad Available");
            },

            (TapsellError error) =>
            {
                // onError
                Debug.Log("Tapsell Error");
                Debug.Log(error.error);
            },

            (string zoneId) =>
            {
                // onNoNetwork
                Debug.Log("No Network");
            },

            (TapsellAd result) =>
            {
                // onExpiring
                Debug.Log("Expiring");
                // this ad is expired, you must download a new ad for this zone
            }
        );
    }

    private void QueryBannerAd()
    {
        Tapsell.requestAd("5ccddc055b60f3000104b7e5", false,
            (TapsellAd result) =>
            {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                _tapsellBanner = result; // store this to show the ad later
            },

            (string zoneId) =>
            {
                // onNoAdAvailable
                Debug.Log("No Ad Available");
            },

            (TapsellError error) =>
            {
                // onError
                Debug.Log("Tapsell Error");
                Debug.Log(error.error);
            },

            (string zoneId) =>
            {
                // onNoNetwork
                Debug.Log("No Network");
            },

            (TapsellAd result) =>
            {
                // onExpiring
                Debug.Log("Expiring");
                // this ad is expired, you must download a new ad for this zone
            }
        );
    }
    private void ShootBannerAd()
    {
        TapsellShowOptions showOptions = new TapsellShowOptions
        {
            backDisabled = false,
            immersiveMode = false,
            rotationMode = TapsellShowOptions.ROTATION_UNLOCKED,
            showDialog = true
        };

        if (_tapsellBanner != null)
        {
            Tapsell.showAd(_tapsellBanner, showOptions);
            LogVideoAdShowed("TapsellBanner");
        }
        
        QueryBannerAd();
    }

    private void ShootTapsellVideo()
    {
        TapsellShowOptions showOptions = new TapsellShowOptions
        {
            backDisabled = false,
            immersiveMode = false,
            rotationMode = TapsellShowOptions.ROTATION_UNLOCKED,
            showDialog = true
        };

        if (_tapSellVideoAd != null)
        {
            Tapsell.showAd(_tapSellVideoAd, showOptions);
            LogVideoAdShowed("Tapsell");
        }

        QueryTapsellVideoAd();
    }

    private void ShootUnityAdsVideo()
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
        Tapsell.initialize("leqinaomplngfjhbdfrgncnnhfcpmopgdjbahoknkfhabhodsorkpiakdprcdnplddmcoq");
        Tapsell.setRewardListener(OnTapsellVideoResult);

        PreloadAds();

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

    private void LogVideoAdShowed(string provider)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("AdDisplayed", "Provider", provider);

        Analytics.CustomEvent("AdDisplayed", new Dictionary<string, object>
        {
            {"Provider", provider}
        });
    }

    private void OnTapsellVideoResult(TapsellAdFinishedResult result)
    {
        if (result.rewarded && result.completed)
        {

        }
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