using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class Quest : Singleton<Quest>
{
    public ObscuredBool[] normalQuest = new ObscuredBool[6];
    public ObscuredBool[] hiddenQuest = new ObscuredBool[6];
    public string[] normalQuestString = new string[6]
    {
        "이렇게도 운이 없네",
        "이렇게도 운이 없네2",
        "이렇게도 운이 없네3",
        "편식이 좋아요",
        "이상한 취미",
        "마지막 기회"
    };
    public string[] hiddenQuestString = new string[6]
    {
        "포카드",
        "풀하우스",
        "스트레이트",
        "투페어",
        "트리플",
        "플러쉬"
    };

    public int[,] questTurretCount = new int[4, 10];

    public CanvasGroup questInfoPanel;
    public Text questInfoText;

    private IEnumerator questFadeCoroutine;

    public void CheckNormalQuest()
    {
        if (normalQuest[0] == false)
        {
            //1성 각 종류 1개씩
            List<int> temp = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                temp.Add(questTurretCount[i, 0]);
            }

            if (QuestMultiValue(temp.ToArray()) != 0)
            {
                Debug.Log("노말 0번 완료");
                QuestInfo(normalQuestString[0], 400);
                normalQuest[0] = true;
                GameManager.instance.money += 400;
            }
        }
        if (normalQuest[1] == false)
        {
            //2성 각 종류 1개씩
            List<int> temp = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                temp.Add(questTurretCount[i, 1]);
            }

            if (QuestMultiValue(temp.ToArray()) != 0)
            {
                Debug.Log("노말 1번 완료");
                QuestInfo(normalQuestString[1], 800);
                normalQuest[1] = true;
                GameManager.instance.money += 800;
            }
        }
        if (normalQuest[2] == false)
        {
            //3성 각 종류 1개씩
            List<int> temp = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                temp.Add(questTurretCount[i, 2]);
            }

            if (QuestMultiValue(temp.ToArray()) != 0)
            {
                Debug.Log("노말 2번 완료");
                QuestInfo(normalQuestString[2], 1600);
                normalQuest[2] = true;
                GameManager.instance.money += 1600;
            }
        }
        if (normalQuest[3] == false)
        {
            //1성 똑같은거 4개
            for (int i = 0; i < 4; i++)
            {
                List<int> temp = new List<int>();

                temp.Add(questTurretCount[i, 0]);
                
                if (QuestAddValue(temp.ToArray()) >= 4)
                {
                    Debug.Log("노말 3번 완료");
                    QuestInfo(normalQuestString[3], 400);
                    normalQuest[3] = true;
                    GameManager.instance.money += 400;
                    break;
                }
            }
        }
        if (normalQuest[4] == false)
        {
            //똑같은 무늬 7개
            for (int i = 0; i < 4; i++)
            {
                List<int> temp = new List<int>();

                for (int j = 0; j < 10; j++)
                {
                    temp.Add(questTurretCount[i, j]);
                }

                if (QuestAddValue(temp.ToArray()) >= 7)
                {
                    Debug.Log("노말 4번 완료");
                    QuestInfo(normalQuestString[4], 1000);
                    normalQuest[4] = true;
                    GameManager.instance.money += 1000;
                    break;
                }
            }
        }
    }

    public void CheckHiddenQuest()
    {
        if(hiddenQuest[0] == false)
        {
            //포카드
            for (int i = 0; i < 4; i++)
            {
                List<int> temp = new List<int>();

                temp.Add(questTurretCount[i, 3]);

                if (QuestAddValue(temp.ToArray()) >= 4)
                {
                    Debug.Log("히든 0번 완료");
                    QuestInfo(hiddenQuestString[0], 4000);
                    hiddenQuest[0] = true;
                    GameManager.instance.money += 4000;
                    PlayerPrefs.SetInt("Hidden6", 0);
                    break;
                }
            }
        }

        if(hiddenQuest[1] == false)
        {
            //풀하우스
            bool clear = false;
            for (int i = 0; i < 4; i++)
            {
                if (clear)
                    break;
                List<int> temp = new List<int>();
                temp.Add(questTurretCount[i, 2]);
                
                if (QuestAddValue(temp.ToArray()) >= 3)
                {
                    for(int j = 0; i < 4; j++)
                    {
                        List<int> temp2 = new List<int>();
                        temp2.Add(questTurretCount[i, 2]);

                        if(QuestAddValue(temp2.ToArray()) >= 2)
                        {
                            Debug.Log("히든 1번 완료");
                            QuestInfo(hiddenQuestString[1], 4000);
                            hiddenQuest[1] = true;
                            clear = true;
                            GameManager.instance.money += 4000;
                            PlayerPrefs.SetInt("Hidden5", 0);
                            break;
                        }
                    }
                }
            }
        }

        if(hiddenQuest[2] == false)
        {
            //스트레이트
            bool clear = false;
            for(int i = 0; i < 4; i++)
            {
                if (clear)
                    break;

                for(int j = 0; j < 5; j++)
                {
                    List<int> temp = new List<int>();
                    for(int k = j; k < j +5; k++)
                    {
                        temp.Add(questTurretCount[i, k]);
                    }

                    if(QuestMultiValue(temp.ToArray()) != 0)
                    {
                        Debug.Log("히든 2번 완료");
                        QuestInfo(hiddenQuestString[2], 5000);
                        hiddenQuest[2] = true;
                        clear = true;
                        GameManager.instance.money += 5000;
                        PlayerPrefs.SetInt("Hidden3", 0);
                    }
                }
            }
        }
        if (hiddenQuest[3] == false)
        {
            //투페어
            bool clear = false;
            for(int i = 0; i < 3; i++)
            {
                if (clear)
                    break;

                List<int> temp = new List<int>();
                temp.Add(questTurretCount[i, 1]);

                for (int j = i + 1; j < 4; j++)
                {
                    List<int> temp2 = new List<int>();
                    temp2.Add(questTurretCount[j, 1]);

                    if(QuestAddValue(temp.ToArray()) >= 2 && QuestAddValue(temp2.ToArray()) >= 2)
                    {
                        Debug.Log("히든 3번 완료");
                        QuestInfo(hiddenQuestString[3], 1000);
                        hiddenQuest[3] = true;
                        clear = true;
                        GameManager.instance.money += 1000;
                        PlayerPrefs.SetInt("Hidden1", 0);
                        break;
                    }
                }
            }
        }

        if(hiddenQuest[4] == false)
        {
            //트리플
            for (int i = 0; i < 4; i++)
            {
                List<int> temp = new List<int>();

                temp.Add(questTurretCount[i, 2]);

                if (QuestAddValue(temp.ToArray()) >= 3)
                {
                    Debug.Log("히든 4번 완료");
                    QuestInfo(hiddenQuestString[4], 2000);
                    hiddenQuest[4] = true;
                    GameManager.instance.money += 2000;
                    PlayerPrefs.SetInt("Hidden2", 0);
                    break;
                }
            }
        }

        if(hiddenQuest[5] == false)
        {
            //플러쉬
            for (int i = 0; i < 4; i++)
            {
                List<int> temp = new List<int>();

                for (int j = 0; j < 10; j++)
                {
                    temp.Add(questTurretCount[i, j]);
                }

                if (QuestAddValue(temp.ToArray()) >= 5)
                {
                    Debug.Log("히든 5번 완료");
                    QuestInfo(hiddenQuestString[5], 1000);
                    hiddenQuest[5] = true;
                    GameManager.instance.money += 1000;
                    PlayerPrefs.SetInt("Hidden4", 0);
                    break;
                }
            }
        }
    }

    public void LastHpMission()
    {
        if(normalQuest[5] == false)
        {
            normalQuest[5] = true;
            GameManager.instance.money += 1000;
            QuestInfo(normalQuestString[5], 1000);
        }
    }

    private int QuestMultiValue(params int[] _value)
    {
        var result = 1;

        for (int i = 0; i < _value.Length; i++)
        {
            result *= _value[i];
        }

        return result;
    }

    private int QuestAddValue(params int[] _value)
    {
        var result = 0;

        for (int i = 0; i < _value.Length; i++)
        {
            result += _value[i];
        }

        return result;
    }

    private void QuestInfo(string _info, int _money)
    {
        questInfoText.text = _info + " 완료" + "\n" +_money.ToString() + "원 획득";

        if(questFadeCoroutine != null)
        {
            StopCoroutine(questFadeCoroutine);
            questFadeCoroutine = null;
        }
        questFadeCoroutine = QuestInfoFade();
        StartCoroutine(questFadeCoroutine);
    }

    private IEnumerator QuestInfoFade()
    {
        questInfoPanel.gameObject.SetActive(true);
        questInfoPanel.alpha = 1;

        while(questInfoPanel.alpha >= 0)
        {
            questInfoPanel.alpha -= Time.deltaTime * 0.5f;
            yield return null;
        }

        questInfoPanel.gameObject.SetActive(false);
        questInfoPanel.alpha = 0;
        questFadeCoroutine = null;
    }
}
