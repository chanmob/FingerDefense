using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    public RectTransform upgradePanel;
    public RectTransform questPanel;

    private bool upgradePanelOn = false;
    private bool questPanelOn = false;

    private ObscuredInt fingerUpgradeCost = 100;
    private ObscuredInt heartUpgradeCost = 100;
    private ObscuredInt spadeUpgradeCost = 100;
    private ObscuredInt cloverUpgradeCost = 100;
    private ObscuredInt diamondUpgradeCost = 100;

    private readonly float[] questCoolTime = new float[5] { 30f, 60f, 90f, 120f, 150f };

    private GameManager gm;

    public GameObject questMonster;
    public GameObject turretInfo;

    public Text turretCostText;
    public Text[] fingerTexts;
    public Text[] heartTexts;
    public Text[] spadeTexts;
    public Text[] cloverTexts;
    public Text[] diamondTexts;

    public Image[] questBars;

    public Button[] questButtons;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void CreateTurretButton()
    {
        if (gm.spawnIdx.Count <= 0)
        {
            return;
        }

        if (gm.money >= (gm.buyTurretCount * 100 + 100))
        {
            gm.money -= (gm.buyTurretCount * 100 + 100);
            gm.MoneyTextRefresh();
            gm.TurretCreated();
            gm.buyTurretCount++;

            turretCostText.text = "터렛구매\n" + (gm.buyTurretCount * 100 + 100);
        }
    }

    public void SellTurretButton()
    {
        StartCoroutine(SellTurretCoroutine());
    }

    public void PauseButton()
    {
        if(Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void GetTurretInfo(Sprite _sprite, string _type, int _lv, int upgrade)
    {
        turretInfo.SetActive(true);

        var turretImg = turretInfo.transform.Find("Image/TurretImg").GetComponent<Image>();
        var turretInfoText = turretInfo.transform.Find("Info").GetComponent<Text>();

        turretImg.sprite = _sprite;
        var power = ((_lv * 2.5f) + 1).ToString() + " + " + upgrade.ToString();
        switch (_type)
        {
            case "CLOVER":
                float freezeamount = 0.1f + (_lv * 0.1f) + (0.1f * upgrade);
                string freeze = "";
                if (freezeamount >= 0.7f)
                    freeze = "0.7";
                else
                    freeze = (0.1f + (_lv * 0.1f)).ToString() + " + " + (0.1f * upgrade).ToString();

                turretInfoText.text = "공격력 : " + power + " / 공격속도 : 0.8" + " / 둔화율 : " + freeze;
                break;
            case "DIAMOND":
                var splash = (1.5f + (_lv * 0.25f)).ToString() + " + " + (0.25f * upgrade).ToString();
                turretInfoText.text = "공격력 : " + power + " / 공격속도 : 0.7" + " / 범위 공격 : " + splash;
                break;
            case "HEART":
                turretInfoText.text = "공격력 : " + power + " / 공격속도 : 0.3";
                break;
            case "SPADE":
                float shockamount = 0.3f + (_lv * 0.05f) + (0.05f * upgrade);
                string shock = "";
                if (shockamount > 0.8f)
                    shock = "0.8";
                else
                    shock = (0.3f + (_lv * 0.05f)).ToString() + " + " + (0.05f * upgrade).ToString();

                turretInfoText.text = "공격력 : " + power + " / 공격속도 : 0.9" + " / 마비 시간 : " + shock;
                break;
        }
    }

    private IEnumerator SellTurretCoroutine()
    {
        GameManager.instance.ReadyToSell();
        GameManager.instance.waitForSale = true;

        yield return new WaitTouch();

        GameManager.instance.waitForSale = false;
        GameManager.instance.FinishToSell();
    }

    public void UpgradeButton()
    {
        if(!upgradePanelOn)
            UIDoTween.instance.UITweenX(upgradePanel, 0, 1f, Ease.OutBack);
        else
            UIDoTween.instance.UITweenX(upgradePanel, -1080, 1f, Ease.OutBack);

        upgradePanelOn = !upgradePanelOn;
    }

    public void QuestButton()
    {
        if (!questPanelOn)
            UIDoTween.instance.UITweenX(questPanel, 0, 1f, Ease.OutBack);
        else
            UIDoTween.instance.UITweenX(questPanel, -1080, 1f, Ease.OutBack);

        questPanelOn = !questPanelOn;
    }

    public void FingerUpgrade()
    {
        if (gm.touchDamage == 1)
        {
            if (gm.money < fingerUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= fingerUpgradeCost;
            }
        }

        else
        {
            if (gm.money < fingerUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= fingerUpgradeCost;
            }
        }

        gm.touchDamage++;
        gm.MoneyTextRefresh();
        fingerUpgradeCost += 100 * gm.touchDamage;
        fingerTexts[0].text = "Lv." + gm.touchDamage.ToString();
        fingerTexts[1].text = fingerUpgradeCost.ToString();
    }

    public void HeartUpgrade()
    {
        if(gm.heartUpgrade == 0)
        {
            if(gm.money < heartUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= heartUpgradeCost;
            }
        }

        else
        {
            if (gm.money < heartUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= heartUpgradeCost;
            }
        }

        gm.heartUpgrade++;
        gm.MoneyTextRefresh();
        heartUpgradeCost += 100 * (gm.heartUpgrade + 1);
        heartTexts[0].text = "Lv." + (gm.heartUpgrade + 1).ToString();
        heartTexts[1].text = heartUpgradeCost.ToString();
    }

    public void SpadeUpgrade()
    {
        if (gm.spadeUpgrade == 0)
        {
            if (gm.money < spadeUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= spadeUpgradeCost;
            }
        }

        else
        {
            if (gm.money < spadeUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= spadeUpgradeCost;
            }
        }

        gm.spadeUpgrade++;
        gm.MoneyTextRefresh();
        spadeUpgradeCost += 100 * (gm.spadeUpgrade + 1);
        spadeTexts[0].text = "Lv." + (gm.spadeUpgrade + 1).ToString();
        spadeTexts[1].text = spadeUpgradeCost.ToString();
    }

    public void CloverUpgrade()
    {
        if (gm.cloverUpgrade == 0)
        {
            if (gm.money < cloverUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= cloverUpgradeCost;
            }
        }

        else
        {
            if (gm.money < cloverUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= cloverUpgradeCost;
            }
        }

        gm.cloverUpgrade++;
        gm.MoneyTextRefresh();
        cloverUpgradeCost += 100 * (gm.cloverUpgrade + 1);
        cloverTexts[0].text = "Lv." + (gm.cloverUpgrade + 1).ToString();
        cloverTexts[1].text = cloverUpgradeCost.ToString();
    }

    public void DiamondUpgrade()
    {
        if (gm.diamondUpgrade == 0)
        {
            if (gm.money < diamondUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= diamondUpgradeCost;
            }
        }

        else
        {
            if (gm.money < diamondUpgradeCost)
            {
                return;
            }

            else
            {
                gm.money -= diamondUpgradeCost;
            }
        }

        gm.diamondUpgrade++;
        gm.MoneyTextRefresh();
        diamondUpgradeCost += 100 * (gm.diamondUpgrade + 1);
        diamondTexts[0].text = "Lv." + (gm.diamondUpgrade + 1).ToString();
        diamondTexts[1].text = diamondUpgradeCost.ToString();
    }

    public void StartQuest(int _idx)
    {
        questButtons[_idx].interactable = false;
        StartCoroutine(QuestCoolTime(_idx, questCoolTime[_idx]));

        var qm = Instantiate(questMonster).GetComponent<QuestMonster>();

        qm.questIdx = _idx + 1;
        qm.HPSetting();
        qm.transform.position = new Vector2(0, 3f);
    }

    private IEnumerator QuestCoolTime(int _idx, float _delay)
    {
        questBars[_idx].fillAmount = 0;
        float time = 0f;

        while(time <= _delay)
        {
            time += Time.deltaTime;

            questBars[_idx].fillAmount = time / _delay;

            yield return null;
        }

        questBars[_idx].fillAmount = 1;
        questButtons[_idx].interactable = true;
    }
}
