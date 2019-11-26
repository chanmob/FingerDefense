using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class Admob : Singleton<Admob>
{
    private readonly string bannerID = "ca-app-pub-9954381112163314/6666860524";
    private readonly string screenID = "ca-app-pub-9954381112163314/3410141798";
    private readonly string rewardID = "ca-app-pub-9954381112163314/7951647640";

    private BannerView banner;
    private InterstitialAd screenAD;
    private RewardBasedVideoAd rewardedAD;

    public ObscuredBool rewarded = false;

    public GameObject noAd;

    void Start()
    {
        InitBannerAD();
        banner.Show();

        if (rewardedAD == null)
            InitRewardedAD();
    }

    public void ShowScreenAD()
    {
        InitScreenAD();
        StartCoroutine(ShowScreenADCoroutine());
    }

    private IEnumerator ShowScreenADCoroutine()
    {
        yield return new WaitForSeconds(1f);

        while (!screenAD.IsLoaded())
        {
            yield return null;
        }

        screenAD.Show();
    }

    private void InitBannerAD()
    {
        banner = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);
    }

    private void InitScreenAD()
    {
        screenAD = new InterstitialAd(screenID);

        AdRequest request = new AdRequest.Builder().Build();

        screenAD.LoadAd(request);
    }

    public void ShowRewardAD()
    {
        RewardCoroutine();

        Debug.Log("보상형 광고 On");
    }

    private void RewardCoroutine()
    {
        if (!rewardedAD.IsLoaded())
        {
            InitRewardedAD();

            if (noAd == null)
                return;

            noAd.SetActive(true);
            Debug.Log("보상형 광고 로드 안됨");
            return;
        }
        else
        {
            Debug.Log("보상형 광고 로드 완료");
        }

        rewardedAD.Show();
    }

    private void InitRewardedAD()
    {
        rewardedAD = RewardBasedVideoAd.Instance;

        AdRequest request = new AdRequest.Builder().Build();

        rewardedAD.LoadAd(request, rewardID);
        rewardedAD.OnAdLoaded += RewardedADLoad;
        rewardedAD.OnAdClosed += RewardedADClose;
        rewardedAD.OnAdRewarded += RewardToUesr;

        Debug.Log("리워드 광고 생성");
    }

    private void RewardedADLoad(object sender, EventArgs arg)
    {
        Debug.Log("보상형 광고 로드");
    }

    private void RewardedADClose(object sender, EventArgs arg)
    {
        Debug.Log("보상형 광고 닫힘");
        InitRewardedAD();
    }

    private void RewardToUesr(object sender, EventArgs arg)
    {
        if(rewarded == false)
        {
            rewarded = true;
            GameManager.instance.ADRestart();
        }

        rewardedAD.OnAdLoaded -= RewardedADLoad;
        rewardedAD.OnAdClosed -= RewardedADClose;
        rewardedAD.OnAdRewarded -= RewardToUesr;
    }
}
