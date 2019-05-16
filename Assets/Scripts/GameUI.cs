using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    public RectTransform upgradePanel;
    public RectTransform questPanel;

    private bool upgradePanelOn = false;
    private bool questPanelOn = false;

    private int buyTurretCount;

    private GameManager gm;

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
            if (gm.money < 100)
            {
                return;
            }

            else
            {
                gm.money -= 100;
            }
        }

        else
        {
            if (gm.money < 100 + (100 * gm.touchDamage))
            {
                return;
            }

            else
            {
                gm.money -= 100 + (100 * gm.touchDamage);
            }
        }

        gm.touchDamage++;

        gm.MoneyTextRefresh();
    }

    public void HeartUpgrade()
    {
        if(gm.heartUpgrade == 0)
        {
            if(gm.money < 100)
            {
                return;
            }

            else
            {
                gm.money -= 100;
            }
        }

        else
        {
            if (gm.money < 100 + (100 * gm.heartUpgrade))
            {
                return;
            }

            else
            {
                gm.money -= 100 + (100 * gm.heartUpgrade);
            }
        }

        gm.heartUpgrade++;

        gm.MoneyTextRefresh();
    }

    public void SpadeUpgrade()
    {
        if (gm.spadeUpgrade == 0)
        {
            if (gm.money < 100)
            {
                return;
            }

            else
            {
                gm.money -= 100;
            }
        }

        else
        {
            if (gm.money < 100 + (100 * gm.spadeUpgrade))
            {
                return;
            }

            else
            {
                gm.money -= 100 + (100 * gm.spadeUpgrade);
            }
        }

        gm.spadeUpgrade++;

        gm.MoneyTextRefresh();
    }

    public void CloverUpgrade()
    {
        if (gm.cloverUpgrade == 0)
        {
            if (gm.money < 100)
            {
                return;
            }

            else
            {
                gm.money -= 100;
            }
        }

        else
        {
            if (gm.money < 100 + (100 * gm.cloverUpgrade))
            {
                return;
            }

            else
            {
                gm.money -= 100 + (100 * gm.cloverUpgrade);
            }
        }

        gm.cloverUpgrade++;

        gm.MoneyTextRefresh();
    }

    public void DiamondUpgrade()
    {
        if (gm.diamondUpgrade == 0)
        {
            if (gm.money < 100)
            {
                return;
            }

            else
            {
                gm.money -= 100;
            }
        }

        else
        {
            if (gm.money < 100 + (100 * gm.diamondUpgrade))
            {
                return;
            }

            else
            {
                gm.money -= 100 + (100 * gm.diamondUpgrade);
            }
        }

        gm.diamondUpgrade++;
        gm.MoneyTextRefresh();
    }
}
