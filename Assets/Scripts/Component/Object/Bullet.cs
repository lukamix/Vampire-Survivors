using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    protected float gravity = 9.8f;
    protected float GetInitialSpeed(float distance, float launchAngle)
    {
        float sine = Mathf.Sin(2 * launchAngle);
        float squaredSpeed = distance * gravity / sine;
        return Mathf.Sqrt(squaredSpeed);
    }
    protected float minAngle = 30f;
    protected int range = 4;
    protected float maxAngle = 90f;
    protected bool isFiring;

    protected float angle;
    protected float speed;
    protected float anglebetween;
    protected float totalTime;
    protected float currentFiringTime = 0;
    protected Vector2 startPoint;
    private Rigidbody2D _rigidbody2D;

    public bool isStraightBullet = true;
    private Vector2 direction; //if is straight;

    public override void SetDame(int level)
    {
        DEFAULT_DAME = 6.5f;
        dameStep = 1f;
        base.SetDame(level);
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isFiring)
        {
            currentFiringTime += Time.fixedDeltaTime;
            if (!isStraightBullet)
            {
                float x = speed * Mathf.Cos(angle);
                float y = speed * Mathf.Sin(angle) - gravity * currentFiringTime;
                Vector2 v = new Vector2(x, y);
                Vector2 rotatedVector = Quaternion.AngleAxis(-anglebetween, Vector3.forward) * v;
                float rotatedangle = Vector2.SignedAngle(Vector2.right, rotatedVector);
                transform.rotation = Quaternion.Euler(Vector3.forward * rotatedangle);
                _rigidbody2D.velocity = rotatedVector;
            }
            else
            {
                _rigidbody2D.velocity = direction * speed;
                float rotatedangle = Vector2.SignedAngle(Vector2.right, direction);
                transform.rotation = Quaternion.Euler(Vector3.forward * rotatedangle);
            }
            CheckLand();
        }
    }
    protected virtual void CheckLand()
    {
        if (currentFiringTime > totalTime)
        {
            isFiring = false;
            currentFiringTime = 0;
            ReturnToPool();
        }
    }
    public void CalculatePosition(Vector2 startPoint, Vector2 endPoint)
    {
        if (!isStraightBullet)
        {
            this.startPoint = startPoint;
            Vector2 offSet = endPoint - startPoint;
            Vector2 direction = offSet.normalized;
            float distance = offSet.magnitude > range ? range : offSet.magnitude;
            distance = distance <= 0 ? 0.01f : distance;
            angle = (maxAngle - (maxAngle - minAngle) / (range) * distance) * Mathf.Deg2Rad;
            speed = GetInitialSpeed(distance, angle);
            totalTime = (distance / (speed * Mathf.Cos(angle)));
            anglebetween = Vector2.SignedAngle(direction, Vector2.right);
        }
        else
        {
            direction = (endPoint - startPoint).normalized;
            speed = 5;
            totalTime = 1;
        }
        isFiring = true;
    }
}
