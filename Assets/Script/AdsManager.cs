using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AdsManager : MonoBehaviour
{
    private SnakeMovement snakeMovement;
    private HardSnakeMovement hardSnakeMovement;
    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
        
        LoadRewardedAd();
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    #region rewarded Ad
    private RewardedAd _rewardedAd;
    private string _adRewardUnitId = "ca-app-pub-1370491335086999/6588892175";
    public void LoadRewardedAd()
  {
      // Clean up the old ad before loading a new one.
      if (_rewardedAd != null)
      {
            _rewardedAd.Destroy();
            _rewardedAd = null;
      }

      Debug.Log("Loading the rewarded ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();

      // send the request to load the ad.
      RewardedAd.Load(_adRewardUnitId, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              _rewardedAd = ad;
          });
            snakeMovement = FindObjectOfType<SnakeMovement>();
            hardSnakeMovement = FindObjectOfType<HardSnakeMovement>();
            Debug.Log(_adRewardUnitId);
  }
  public void ShowRewardedAd()
    {
    const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

    if (_rewardedAd != null && _rewardedAd.CanShowAd())
    {
        _rewardedAd.Show((Reward reward) =>
        {
            int currentSceneIndex = GetCurrentSceneIndex();
            // Sahne 1 veya 3 ise ShowRewardedAd() metodunu çağır
        if (currentSceneIndex == 1 || currentSceneIndex == 3)
        {
            snakeMovement.ContinueFromLastCheckpoint();
        }
        // Sahne 2 veya 4 ise HardContinueFromLastCheckpoint() metodunu çağır
        else if (currentSceneIndex == 2 || currentSceneIndex == 4)
        {
            hardSnakeMovement.HardContinueFromLastCheckpoint();
        }
            //ToDo
            Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
        });
        RegisterReloadHandler(_rewardedAd);
    }
    }
    private void RegisterReloadHandler(RewardedAd ad)
{
    // Raised when the ad closed full screen content.
    ad.OnAdFullScreenContentClosed += () =>
    {
        Debug.Log("Rewarded Ad full screen content closed.");

        // Reload the ad so that we can show another as soon as possible.
        LoadRewardedAd();
    };
    // Raised when the ad failed to open full screen content.
    ad.OnAdFullScreenContentFailed += (AdError error) =>
    {
        Debug.LogError("Rewarded ad failed to open full screen content " +
                       "with error : " + error);

        // Reload the ad so that we can show another as soon as possible.
        LoadRewardedAd();
    };
}

    #endregion
}
