using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerTurret : MonoBehaviour
{
    public enum TURRETTYPE
    {
        NONE,
        HEART,
        SPADE,
        CLOVER,
        DIAMOND
    }

    public TURRETTYPE turretType = TURRETTYPE.NONE;

    public int power;
    public int turretLevel;
    public int turretPositionIndex;

    public float timeBetFire;
    private const float levelUpMinDistance = 1f;

    private GameObject target;

    private Vector3 mousePosition;
    private Vector3 offset;
    private Vector3 pos;

    private void OnMouseDown()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = this.transform.position - mousePosition;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePosition + offset;
    }

    private void OnMouseUp()
    {
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
                GameManager.instance.DestroyTurret(this);
                pt.turretLevel++;
                nearTurret.GetComponent<SpriteRenderer>().sprite = GameManager.instance.ChangeCardSprite(turretType.ToString(), turretLevel + 1);
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
        pos = this.transform.position;
        InvokeRepeating("FindEnemy", 0f, 0.5f);
    }

    public void FindEnemy()
    {
        target = null;

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
            target = nearestMonster;
            target.GetComponent<Monster>().OnDamage(power);
        }
    }
}
