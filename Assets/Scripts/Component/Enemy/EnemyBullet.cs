using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : ItemPool
{
    private Rigidbody2D _rigidBody2D;
    public Rigidbody2D rgbd2D
    {
        get
        {
            if (_rigidBody2D == null) _rigidBody2D = GetComponent<Rigidbody2D>();
            return _rigidBody2D;
        }
    }
    private Transform target;
    private float speed = 2f;
    private float dame;
    public void SetDame(float value)
    {
        dame = value;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Character")))
        {
            Attribute att = collision.gameObject.GetComponent<Attribute>();
            att.GetDame(dame);
            ReturnToPool();
        }
        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("King"))){
            DaiBang db = collision.GetComponent<DaiBang>();
            db.GetDamage(dame);
            ReturnToPool();
        }
        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Guard")))
        {
            Guard guard = collision.GetComponent<Guard>();
            guard.GetDamage();
            ReturnToPool();
        }
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rgbd2D.velocity = speed * direction;
        }
    }
}
