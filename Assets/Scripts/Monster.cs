﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IDamageable
{
    public ObscuredInt maxHp;
    public ObscuredInt curHp;
    public ObscuredFloat curSpeed;
    public ObscuredFloat speed = 1f;

    public ObscuredBool isFreeze = false;

    public Image healthBar;

    private void OnEnable()
    {
        var gm = GameManager.instance;
        maxHp = gm.currentWave * ((gm.currentWave / 10) + 1);
        curHp = maxHp;
        curSpeed = speed;
        //보스는 제곱
    }

    private void OnDisable()
    {
        GameManager.instance.CheckWaveIsEnd();
        speed = 1f;
        healthBar.fillAmount = 1;
    }

    public void OnMouseDown()
    {
        OnDamage(GameManager.instance.touchDamage);
    }

    public void OnDamage(int _damage)
    {
        curHp -= _damage;
        healthBar.fillAmount = (float)curHp / (float)maxHp;

        if(curHp <= 0)
        {
            GameManager.instance.money += 100;
            GameManager.instance.MoneyTextRefresh();
            GameManager.instance.DisableMonster(this.gameObject);
        }
    }

    private void Update()
    {
        this.transform.Translate(Vector2.down * curSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            GameManager.instance.DisableMonster(this.gameObject);
        }
    }

    public void Freeze(float _slowAmount)
    {
        if (isFreeze)
            return;

        StartCoroutine(FreezeCoroutine(_slowAmount));
    }

    private IEnumerator FreezeCoroutine(float _slowAmount)
    {
        isFreeze = true;
        curSpeed = curSpeed * (1 - _slowAmount);

        yield return new WaitForSeconds(1f);

        curSpeed = speed;
        isFreeze = false;
    }
}
