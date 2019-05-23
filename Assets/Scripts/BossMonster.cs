using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
    public override void MonsterHpReset()
    {
        var gm = GameManager.instance;
        maxHp = (int)Mathf.Pow((gm.currentWave / 10),2) * 100;
        curHp = maxHp;
        curSpeed = speed;
    }

    public override void MonsterStatusReset()
    {
        EventManager.instance.waitForWaveToEndHandler();       
    }

    public override void MonsterDie()
    {
        isDie = true;
        GameManager.instance.money += GameManager.instance.currentWave * 100;
        GameManager.instance.MoneyTextRefresh();
        Destroy(this.gameObject);
    }

    public override void MonsterArriveDeadZone()
    {
        GameManager.instance.currenthp--;
        GameManager.instance.coinAudio.PlayOneShot(GameManager.instance.hitAudioClip);
        GameManager.instance.CheckEndGame();
        Destroy(this.gameObject);
    }
}
