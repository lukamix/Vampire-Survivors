using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPiece : MonoBehaviour
{
    private float speed = 5;
    private float flyRange = 5;
    private float flyAngle;
    private bool isFlying = false;
    private float currentDistance = 0 ;
    private Vector2 offSet;
    private Rigidbody2D _rigidbody2D;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (isFlying)
        {
            currentDistance += speed * Time.fixedDeltaTime;
            _rigidbody2D.velocity = speed * new Vector2(Mathf.Cos(flyAngle), Mathf.Sin(flyAngle));
            if (currentDistance >= flyRange)
            {
                isFlying = false;
                currentDistance = 0;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.layer.Equals(LayerMask.NameToLayer("Character")))
        {
            Destroy(gameObject);
        }
    }
    public void Fly(float flyAngle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, flyAngle));
        this.flyAngle = flyAngle * Mathf.Deg2Rad;
        isFlying = true;
    }
}
