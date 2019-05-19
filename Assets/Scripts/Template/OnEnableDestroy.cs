using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class OnEnableDestroy : MonoBehaviour
{
    private SpriteRenderer sp;
    public ObscuredFloat damage;
    public GameObject target;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        
        StartCoroutine(Fade());
        StartCoroutine(WaitDestroy(0.2f));  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject == target)
                return;

            collision.GetComponent<IDamageable>().OnDamage(damage);
        }
    }

    IEnumerator WaitDestroy(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        Destroy(this.gameObject);
    }

    IEnumerator Fade()
    {
        float t = 1.0f;

        while (t >= 0)
        {
            t -= Time.deltaTime / 0.1f;

            sp.color = new Color(1, 1, 1, t);

            yield return null;
        }
    }
}
