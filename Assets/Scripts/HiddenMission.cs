using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenMission : MonoBehaviour
{
    public Text missionText;

    private bool[] showHiddenMission = new bool[6];
    private string[] hiddenMission = new string[6]
    {
        "투페어 : 2레벨 2종류 카드 2장씩 모으기",
        "트리플 : 3레벨 똑같은 카드 3장 모으기",
        "스트레이트 : 한 종류의 연속된 숫자 카드 5장 모으기",
        "플러쉬 : 같은 종류의 카드 5장 모으기",
        "풀하우스 : 3레벨 2종류 카드 2, 3장씩 모으기",
        "포카드 : 4레벨 똑같은 카드 4장 모으기"
    };

    private void Start()
    {
        if (PlayerPrefs.HasKey("Hidden1"))
        {
            showHiddenMission[0] = true;
        }
        if (PlayerPrefs.HasKey("Hidden2"))
        {
            showHiddenMission[1] = true;
        }
        if (PlayerPrefs.HasKey("Hidden3"))
        {
            showHiddenMission[2] = true;
        }
        if (PlayerPrefs.HasKey("Hidden4"))
        {
            showHiddenMission[3] = true;
        }
        if (PlayerPrefs.HasKey("Hidden5"))
        {
            showHiddenMission[4] = true;
        }
        if (PlayerPrefs.HasKey("Hidden6"))
        {
            showHiddenMission[5] = true;
        }

        for(int i = 0; i < showHiddenMission.Length; i++)
        {
            if(showHiddenMission[i] == false)
            {
                hiddenMission[i] = "???";
            }
        }

        missionText.text = "- 일반 미션 -\n" +
            "이렇게도 운이 없네 : 1레벨 모든 카드 모으기\n" +
            "이렇게도 운이 없네2 : 2레벨 모든 카드 모으기\n" +
            "이렇게도 운이 없네3 : 3레벨 모든 카드 모으기\n" +
            "편식이 좋아요 : 1레벨 똑같은 카드 4장 모으기\n" +
            "이상한 취미 : 같은 종류의 카드 7장 모으기\n" +
            "마지막 기회 : 플레이어 체력 1 달성\n\n" +
            "- 히든 미션 -\n" +
            hiddenMission[0] + "\n" +
            hiddenMission[1] + "\n" +
            hiddenMission[2] + "\n" +
            hiddenMission[3] + "\n" +
            hiddenMission[4] + "\n" +
            hiddenMission[5];
    }
}
