using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Admob : Singleton<Admob>
{
    private readonly string bannerID = "ca-app-pub-9954381112163314/6666860524";
    private readonly string screenID = "ca-app-pub-9954381112163314/3410141798";

    private BannerView banner;
    private InterstitialAd screenAD;

    void Start()
    {
        InitBannerAD();
        banner.Show();
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
}
