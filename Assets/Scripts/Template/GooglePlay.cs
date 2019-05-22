using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class GooglePlay : Singleton<GooglePlay>
{
    private void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public bool GooglePlayLogine()
    {
        bool result = false;

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            });
        }
        else
        {
            result = true;
        }

        return result;
    }

    public void ShowRanking()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard);
    }

    public void UploadRanking(int _score)
    {
        Social.ReportScore((long)_score, GPGSIds.leaderboard, null);
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void GetAchievement(string _id)
    {
        Social.ReportProgress(_id, 100f, null);
    }
}
