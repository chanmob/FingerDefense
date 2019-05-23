using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class QuestMonster : Monster
{
    public int questIdx = 0;

    public override void MonsterArriveDeadZone()
    {
        GameManager.instance.currenthp -= questIdx;
        GameManager.instance.coinAudio.PlayOneShot(GameManager.instance.hitAudioClip);
        GameManager.instance.CheckEndGame();
        Destroy(this.gameObject);
    }

    public override void MonsterHpReset()
    {
        isDie = false;
        curSpeed = 2;
    }

    public void HPSetting()
    {
        maxHp = Mathf.Pow(questIdx, 2) * 150;
        curHp = maxHp;
    }

    public override void MonsterStatusReset()
    {
        
    }

    public override void MonsterDie()
    {
        isDie = true;
        GameManager.instance.money += (ObscuredInt)Mathf.Pow(questIdx, 2) * 100;
        GameManager.instance.MoneyTextRefresh();
        Destroy(this.gameObject);
    }
}
