using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public Transform target;
    [HideInInspector] public float speed, damage;
    private Vector3 currentDirection;

    private void Update()
    {
        Vector3 newDirection = target ? (target.position - transform.position).normalized
                                      : currentDirection;
        transform.position += newDirection * speed * Time.deltaTime;
        currentDirection = newDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyManager.instance.TakeDamage(collision.gameObject.GetComponent<Enemy>(), damage);
            Destroy(gameObject);
        }
    }
}
