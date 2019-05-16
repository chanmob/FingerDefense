using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public int hp;
    public float speed = 1f;

    public void OnMouseDown()
    {
        OnDamage(1);
    }

    public void OnDamage(int _damage)
    {
        hp -= _damage;

        if(hp <= 0)
        {
            GameManager.instance.DisableMonster(this.gameObject);
        }
    }

    private void Update()
    {
        this.transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            GameManager.instance.DisableMonster(this.gameObject);
        }
    }
}
