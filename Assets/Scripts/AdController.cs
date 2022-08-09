using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

public class AdController : MonoBehaviour
{
#if UNITY_IOS
    public const string gameID = "3196407";
#elif UNITY_ANDROID
    public const string gameID = "3196406";
#endif

    public bool testMode = true;
    string placementID;

    GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        Monetization.Initialize(gameID, testMode);
    }

    public void ShowRewardedAd()
    {
        placementID = "rewardedVideo";
        StartCoroutine(ShowAd());
    }

    private IEnumerator ShowAd()
    {
        while (!Monetization.IsReady(placementID))
        {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(placementID) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show(AdCallBack);
        }
    }

    private void AdCallBack(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            print("Finished");
            gameController.rewardedAdWatched = true;
        }
    }
}

