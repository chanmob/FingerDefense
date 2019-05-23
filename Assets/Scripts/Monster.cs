using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IDamageable
{
    public ObscuredInt shockSpeed = 1;

    public ObscuredFloat maxHp;
    public ObscuredFloat curHp;
    public ObscuredFloat curSpeed;
    public ObscuredFloat speed = 1f;
    private ObscuredFloat lastFreezeTime;
    private float freezeTime = 1f;

    public ObscuredBool isFreeze = false;
    public ObscuredBool isShock = false;
    public ObscuredBool isDie = false;
    
    public Image healthBar;

    public IEnumerator shockCoroutine;

    private AudioSource audioSource;
    public AudioClip fingerAttackClip;
    public AudioClip moneyClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        MonsterHpReset();
    }

    private void OnDisable()
    {
        MonsterStatusReset();
    }

    public virtual void MonsterHpReset()
    {
        isDie = false;
        var gm = GameManager.instance;
        maxHp = gm.currentWave * ((gm.currentWave / 10) + 1);
        curHp = maxHp;
        curSpeed = speed;
    }

    public virtual void MonsterStatusReset()
    {
        GameManager.instance.CheckWaveIsEnd();
        speed = 1f;
        healthBar.fillAmount = 1;
        shockSpeed = 1;
        isShock = false;
        isFreeze = false;
        if(shockCoroutine != null)
        {
            StopCoroutine(shockCoroutine);
            shockCoroutine = null;
        }
    }

    public void OnMouseDown()
    {
        audioSource.PlayOneShot(fingerAttackClip);
        OnDamage(GameManager.instance.touchDamage);
    }

    public void OnDamage(float _damage)
    {
        if (isDie)
            return;

        curHp -= _damage;
        healthBar.fillAmount = (float)curHp / (float)maxHp;

        if(curHp <= 0)
        {
            GameManager.instance.coinAudio.PlayOneShot(moneyClip);
            MonsterDie();
        }
    }

    public virtual void MonsterDie()
    {
        if (isDie)
            return;

        isDie = true;
        GameManager.instance.money += 100;
        GameManager.instance.MoneyTextRefresh();
        GameManager.instance.DisableMonster(this.gameObject);
    }

    private void Update()
    {
        if(isFreeze && Time.time >= lastFreezeTime + freezeTime)
        {
            curSpeed = speed;
            isFreeze = false;
        }

        if(curSpeed <= speed * 0.5f)
        {
            curSpeed = speed * 0.5f;
        }

        this.transform.Translate(Vector2.down * curSpeed * Time.deltaTime * shockSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            MonsterArriveDeadZone();
        }
    }

    public virtual void MonsterArriveDeadZone()
    {
        GameManager.instance.currenthp--;
        GameManager.instance.coinAudio.PlayOneShot(GameManager.instance.hitAudioClip);
        GameManager.instance.CheckEndGame();
        GameManager.instance.DisableMonster(this.gameObject);
    }

    public void Freeze(float _slowAmount)
    {
        isFreeze = true;

        lastFreezeTime = Time.time;

        curSpeed = curSpeed * (1 - _slowAmount);
    }

    public void Shock()
    {
        if (isShock || this.gameObject.activeSelf == false)
            return;

        shockCoroutine = ShockCoroutine();
        StartCoroutine(shockCoroutine);
    }

    private IEnumerator ShockCoroutine()
    {
        isShock = true;
        shockSpeed = 0;

        yield return new WaitForSeconds(0.3f);

        shockSpeed = 1;

        yield return new WaitForSeconds(0.6f);
        isShock = false;
    }
}
