using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ItemPool
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollide)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("CharacterInteractItem")))
            {
                target = collision.transform.parent.parent;
                isCollide = true;
            }
        }
    }
    private bool isCollide = false;
    private Transform target;

    private float minDistance = 0.1f;
    private float maxSpeed = 20;
    private float speed = -3;
    private float accelerate = 0.4f;
    private void FixedUpdate()
    {
        FlyToTarget();
    }
    private void FlyToTarget()
    {
        if (isCollide)
        {
            Vector2 v = target.position - transform.position;
            float distance = v.magnitude;
            if (distance < minDistance)
            {
                isCollide = false;
                Attribute atrribute = target.GetComponent<Attribute>();
                atrribute.OnCollideItem(this);
                ReturnToPool();
            }
            else
            {
                Vector2 direction = v.normalized;
                if (speed < maxSpeed)
                {
                    speed += accelerate;
                }
                transform.position = (Vector2)transform.position + speed * direction * Time.fixedDeltaTime;
            }
        }
    }
}
