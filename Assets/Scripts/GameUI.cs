using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public RectTransform upgradePanel;
    public RectTransform questPanel;

    private bool upgradePanelOn = false;
    private bool questPanelOn = false;

    private int buyTurretCount;
    private ObscuredInt fingerUpgradeCost = 100;
    private ObscuredInt heartUpgradeCost = 100;
    private ObscuredInt spadeUpgradeCost = 100;
    private ObscuredInt cloverUpgradeCost = 100;
    private ObscuredInt diamondUpgradeCost = 100;


    private GameManager gm;

    public Text turretCostText;
    public Text[] fingerTexts;
    public Text[] heartTexts;
    public Text[] spadeTexts;
    public Text[] cloverTexts;
    public Text[] diamondTexts;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void CreateTurretButton()
    {
        if(gm.money >= (buyTurretCount * 100 + 100))
        {
            gm.money -= (buyTurretCount * 100 + 100);
            gm.MoneyTextRefresh();
            gm.TurretCreated();
            buyTurretCount++;

            turretCostText.text = "터렛구매\n" + (buyTurretCount * 100 + 100);
        }
    }

    public void SellTurretButton()
    {

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
        fingerUpgradeCost += 100 + (100 * gm.touchDamage);
        fingerTexts[0].text = "Lv." + gm.touchDamage;
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
        heartUpgradeCost += 100 + (100 * gm.heartUpgrade + 1);
        heartTexts[0].text = "Lv." + gm.heartUpgrade + 1;
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
        spadeUpgradeCost += 100 + (100 * gm.spadeUpgrade + 1);
        spadeTexts[0].text = "Lv." + gm.spadeUpgrade + 1;
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
        cloverUpgradeCost += 100 + (100 * gm.cloverUpgrade + 1);
        cloverTexts[0].text = "Lv." + gm.cloverUpgrade + 1;
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
        diamondUpgradeCost += 100 + (100 * gm.diamondUpgrade + 1);
        diamondTexts[0].text = "Lv." + gm.diamondUpgrade + 1;
        diamondTexts[1].text = diamondUpgradeCost.ToString();
    }
}
