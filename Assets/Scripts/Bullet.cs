using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public ObscuredFloat speed;
    public ObscuredInt damage;

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
        }

        if(target.activeSelf == false)
        {
            GameManager.instance.DisableBullet(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == target)
        {
            collision.GetComponent<IDamageable>().OnDamage(damage);
            GameManager.instance.DisableBullet(this.gameObject);
        }
    }
}
