using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }

    public void Archieve()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
