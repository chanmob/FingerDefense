using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class MainUI : MonoBehaviour
{
    private bool soundOff = false;
    public Image soundImage;
    public Sprite[] soundSprites;

    private void Start()
    {
        if (PlayerPrefs.HasKey("SOUND"))
        {
            var sound = PlayerPrefs.GetString("SOUND");

            if (sound == "true")
            {
                soundOff = false;
                AudioListener.volume = 1;
            }
            else if (sound == "false")
            {
                soundOff = true;
                AudioListener.volume = 0;
            }
        }
    }

    public void Review()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ChanMob.FingerDefense");
    }

    public void SoundControl()
    {
        if (soundOff)
        {
            soundOff = false;
            AudioListener.volume = 1;
            soundImage.sprite = soundSprites[0];
            PlayerPrefs.SetString("SOUND", soundOff.ToString());
        }

        else
        {
            soundOff = true;
            AudioListener.volume = 0;
            soundImage.sprite = soundSprites[1];
            PlayerPrefs.SetString("SOUND", soundOff.ToString());
        }
    }

    public void Ranking()
    {
        int bestScore = 0;
        int uploadScore = 0;

        if (ObscuredPrefs.HasKey("BESTSCORE"))
        {
            bestScore = ObscuredPrefs.GetInt("BESTSCORE");
        }

        if (ObscuredPrefs.HasKey("UPLOADSCORE"))
        {
            uploadScore = ObscuredPrefs.GetInt("UPLOADSCORE");
        }

        if(bestScore != uploadScore)
        {
            uploadScore = bestScore;
            ObscuredPrefs.SetInt("UPLOADSCORE", uploadScore);

            if (GooglePlay.instance.GooglePlayLogine())
            {
                GooglePlay.instance.UploadRanking(uploadScore);
                GooglePlay.instance.ShowRanking();
            }
        }
        else
        {
            if (GooglePlay.instance.GooglePlayLogine())
                GooglePlay.instance.ShowRanking();
        }
    }

    public void Achieve()
    {
        if (GooglePlay.instance.GooglePlayLogine())
            GooglePlay.instance.ShowAchievements();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
