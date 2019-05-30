using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class PokerTurret : MonoBehaviour
{
    public enum TURRETTYPE
    {
        NONE,
        CLOVER,
        DIAMOND,
        HEART,
        SPADE
    }

    public TURRETTYPE turretType = TURRETTYPE.NONE;

    public ObscuredFloat power;
    public ObscuredInt turretLevel;
    public ObscuredInt turretPositionIndex;

    public ObscuredFloat timeBetFire;
    private const float levelUpMinDistance = 1f;

    public GameObject bullet;

    private Vector3 mousePosition;
    private Vector3 offset;
    private Vector3 pos;

    public AudioSource audioSource;

    public AudioClip attackAudioClip;
    public AudioClip createdAudioClip;
    public AudioClip collaboAudioClip;

    private void OnMouseDown()
    {
        if (GameManager.instance.waitForSale)
        {
            GameManager.instance.DestroyTurret(this);
            GameManager.instance.createdPosition[turretPositionIndex] = false;
            Quest.instance.questTurretCount[(int)turretType - 1, turretLevel] -= 1;
            GameManager.instance.money += (GameManager.instance.buyTurretCount * 100 + 100) / 2;
            GameManager.instance.MoneyTextRefresh();
        }

        else
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = this.transform.position - mousePosition;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePosition + offset;
    }

    private void OnMouseUp()
    {
        if (Vector2.Distance(pos, mousePosition) <= 0.5f)
        {
            Debug.Log("정보 띄우기");
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;

            switch (turretType)
            {
                case TURRETTYPE.CLOVER:
                    GameUI.instance.GetTurretInfo(sprite, turretType.ToString(), turretLevel, GameManager.instance.cloverUpgrade);
                    break;
                case TURRETTYPE.DIAMOND:
                    GameUI.instance.GetTurretInfo(sprite, turretType.ToString(), turretLevel, GameManager.instance.diamondUpgrade);
                    break;
                case TURRETTYPE.HEART:
                    GameUI.instance.GetTurretInfo(sprite, turretType.ToString(), turretLevel, GameManager.instance.heartUpgrade);
                    break;
                case TURRETTYPE.SPADE:
                    GameUI.instance.GetTurretInfo(sprite, turretType.ToString(), turretLevel, GameManager.instance.spadeUpgrade);
                    break;
            }
        }

        var turrets = GameObject.FindGameObjectsWithTag("Turret");
        float minDiff = Mathf.Infinity;
        GameObject nearTurret = null;

        foreach(var turret in turrets)
        {
            if(turret == this.gameObject)
            {
                continue;
            }

            var diff = Vector2.Distance(this.transform.position, turret.transform.position);

            if(diff < minDiff)
            {
                minDiff = diff;
                nearTurret = turret;
            }
        }

        if (minDiff <= levelUpMinDistance && nearTurret != null)
        {
            PokerTurret pt = nearTurret.GetComponent<PokerTurret>();

            if (turretType == pt.turretType && this.turretLevel == pt.turretLevel)
            {
                Debug.Log("합체 성공");
                if (GameUI.instance.turretInfo.activeSelf == true)
                    GameUI.instance.turretInfo.SetActive(false);

                GameManager.instance.createdPosition[turretPositionIndex] = false;
                pt.turretLevel++;
                int randomTurret = Random.Range(0, GameManager.instance.turrets.Length);

                switch (randomTurret)
                {
                    case 0:
                        pt.turretType = TURRETTYPE.CLOVER;
                        break;
                    case 1:
                        pt.turretType = TURRETTYPE.DIAMOND;
                        break;
                    case 2:
                        pt.turretType = TURRETTYPE.HEART;
                        break;
                    case 3:
                        pt.turretType = TURRETTYPE.SPADE;
                        break;
                }

                nearTurret.GetComponent<SpriteRenderer>().sprite = GameManager.instance.ChangeCardSprite(pt.turretType.ToString(), turretLevel + 1);
                nearTurret.GetComponent<Animator>().SetTrigger("Coalescence");
                pt.audioSource.PlayOneShot(pt.collaboAudioClip);
                pt.Collabo();
                Quest.instance.questTurretCount[(int)turretType - 1, turretLevel] -= 2;
                Quest.instance.questTurretCount[randomTurret, turretLevel + 1] += 1;
                Quest.instance.CheckHiddenQuest();
                Quest.instance.CheckNormalQuest();
                GameManager.instance.DestroyTurret(this);
            }
            else
            {
                Debug.Log("합체 실패");
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                this.transform.position = pos;
            }
        }
        else
        {
            Debug.Log("합체 실패");
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            this.transform.position = pos;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pos = this.transform.position;
        audioSource.PlayOneShot(createdAudioClip);
        InvokeRepeating("FindEnemy", 0f, timeBetFire);
    }

    public void Collabo()
    {
        CancelInvoke("FindEnemy");

        switch (turretType)
        {
            case TURRETTYPE.CLOVER:
                timeBetFire = 0.7f;
                break;
            case TURRETTYPE.DIAMOND:
                timeBetFire = 0.7f;
                break;
            case TURRETTYPE.HEART:
                timeBetFire = 0.3f;
                break;
            case TURRETTYPE.SPADE:
                timeBetFire = 0.7f;
                break;
        }

        InvokeRepeating("FindEnemy", 0f, timeBetFire);
    }

    public void FindEnemy()
    {
        var enemys = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestMonster = null;

        foreach(var enemy in enemys)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestMonster = enemy;
            }
        }

        if(nearestMonster != null)
        {
            var bulletPrefab = GameManager.instance.GetBullet(turretType.ToString());
            var b = bulletPrefab.GetComponent<Bullet>();
            b.target = nearestMonster;
            if(string.IsNullOrEmpty(b.type))
            {
                b.type = turretType.ToString();
            }
            power = (turretLevel * 2.5f) + 1;
            b.level = turretLevel;
            var gm = GameManager.instance;
            switch (turretType)
            {
                case TURRETTYPE.CLOVER:
                    b.speed = 15f;
                    b.damage = power + gm.cloverUpgrade;
                    break;
                case TURRETTYPE.DIAMOND:
                    b.speed = 15f;
                    b.damage = power + gm.diamondUpgrade;
                    break;
                case TURRETTYPE.HEART:
                    b.speed = 15f;
                    b.damage = power + gm.heartUpgrade;
                    break;
                case TURRETTYPE.SPADE:
                    b.damage = power + gm.spadeUpgrade;
                    b.speed = 15f;
                    break;
            }
            audioSource.PlayOneShot(attackAudioClip);
            bulletPrefab.transform.position = this.transform.position;
            bulletPrefab.SetActive(true);
        }
    }
}
