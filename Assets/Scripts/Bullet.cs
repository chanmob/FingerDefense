using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public ObscuredInt level;
    public ObscuredFloat speed;
    public ObscuredFloat damage;
    public string type;

    public GameObject diaBulletEffect;

    private void OnDisable()
    {
        target = null;
        speed = 0f;
        damage = 0;
    }

    private void Update()
    {
        if(target != null)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);

            if (target.activeSelf == false)
            {
                GameManager.instance.DisableBullet(this.gameObject);
            }
        }

        else
        {
            GameManager.instance.DisableBullet(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == target)
        {
            switch (type)
            {
                case "HEART":
                    break;
                case "DIAMOND":
                    var diaBullet = Instantiate(diaBulletEffect, collision.transform.position ,Quaternion.identity);
                    diaBullet.transform.localScale = new Vector3(
                        1.5f + EffectAmount(level, GameManager.instance.diamondUpgrade, 0.25f),
                        1.5f + EffectAmount(level, GameManager.instance.diamondUpgrade, 0.25f),
                        1);
                    var effect = diaBullet.GetComponent<OnEnableDestroy>();
                    effect.damage = damage * 0.5f;
                    effect.target = collision.gameObject;
                    break;
                case "CLOVER":
                    collision.GetComponent<Monster>().Freeze(EffectAmount(level, GameManager.instance.cloverUpgrade, 0.1f) + 0.1f);
                    break;
                case "SPADE":
                    collision.GetComponent<Monster>().Shock(EffectAmount(level, GameManager.instance.spadeUpgrade, 0.05f) + 0.3f);
                    break;
            }

            collision.GetComponent<IDamageable>().OnDamage(damage);
            GameManager.instance.DisableBullet(this.gameObject);
        }
    }

    private float EffectAmount(int lv, int upgrade, float multi)
    {
        float result = 0;
        result = (lv * multi) + (upgrade * multi);
        return result;
    }
}
