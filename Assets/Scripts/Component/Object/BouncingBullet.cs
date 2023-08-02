using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Weapon
{
    protected bool isFiring;
    protected float currentFiringTime = 0;
    protected float speed = 3.5f;
    protected float totalTime = 5;

    private Vector2 direction;
    private Transform player;

    private Rigidbody2D _rigidbody2D;
    public override void SetDame(int level)
    {
        DEFAULT_DAME = 5;
        dameStep = 1;
        base.SetDame(level);
    }
    private void Start()
    {        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (isFiring)
        {
            currentFiringTime += Time.fixedDeltaTime;
            _rigidbody2D.velocity = direction * speed;
            CheckCollide();
            CheckLand();
        }
    }
    public void SetPlayer(Transform pl)
    {
        player = pl;
        isFiring = true;
    }
    public void SetDirection(int dir)
    {
        float angle = Mathf.Atan(9f / 16f);
        float x = speed * Mathf.Cos(angle);
        float y = speed * Mathf.Sin(angle);
        if (dir == (int)BouncingBulletDirection.TOPLEFT)
        {
            direction = new Vector2(-x, y);
        }
        else if(dir == (int)BouncingBulletDirection.TOPRIGHT)
        {
            direction = new Vector2(x, y);
        }
        else if (dir == (int)BouncingBulletDirection.BOTTOMLEFT)
        {
            direction = new Vector2(-x, -y);
        }
        else
        {
            direction = new Vector2(x, -y);
        }
        SetRotation();
    }
    private void SetRotation()
    {
        float rotatedangle = Vector2.SignedAngle(Vector2.right, direction);
        transform.rotation = Quaternion.Euler(Vector3.forward * rotatedangle);
    }
    private void CheckCollide()
    {
        if (transform.position.x >= player.transform.position.x + MAX_X_DISTANCE_FROM_PLAYER && direction.x>0)
        {
            direction = new Vector2(-direction.x, direction.y);
        }
        else if(transform.position.x<=player.transform.position.x - MAX_X_DISTANCE_FROM_PLAYER && direction.x<0)
        {
            direction = new Vector2(-direction.x, direction.y);
        }
        else if(transform.position.y>= player.transform.position.y + MAX_Y_DISTANCE_FROM_PLAYER && direction.y>0)
        {
            direction = new Vector2(direction.x, -direction.y);
        }
        else if(transform.position.y<= player.transform.position.y - MAX_Y_DISTANCE_FROM_PLAYER && direction.y<0)
        {
            direction = new Vector2(direction.x, -direction.y);
        }
        SetRotation();
    }
    private void CheckLand()
    {
        if (currentFiringTime > totalTime)
        {
            isFiring = false;
            currentFiringTime = 0;
            ReturnToPool();
        }
    }
}
public enum BouncingBulletDirection
{
    TOPLEFT = 0,
    TOPRIGHT = 1,
    BOTTOMLEFT = 2,
    BOTTOMRIGHT = 3
}