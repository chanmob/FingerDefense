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
        Admob.instance.ShowBannerAD();

        if (ObscuredPrefs.HasKey("SOUND"))
        { 
            var sound = ObscuredPrefs.GetInt("SOUND");
            Debug.Log("SOUND 키 존재 : " + sound);

            if (sound == 1)
            {
                soundOff = true;
                AudioListener.volume = 0;
                soundImage.sprite = soundSprites[1];
                Debug.Log(soundOff + " 로딩 완료");
            }
            else if (sound == 0)
            {
                soundOff = false;
                AudioListener.volume = 1;
                soundImage.sprite = soundSprites[0];
                Debug.Log(soundOff + " 로딩 완료");
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
            ObscuredPrefs.SetInt("SOUND", 0);

            Debug.Log(ObscuredPrefs.GetInt("SOUND") + " 저장완료");
        }

        else
        {
            soundOff = true;
            AudioListener.volume = 0;
            soundImage.sprite = soundSprites[1];
            ObscuredPrefs.SetInt("SOUND", 1);

            Debug.Log(ObscuredPrefs.GetInt("SOUND") + " 저장완료");
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
