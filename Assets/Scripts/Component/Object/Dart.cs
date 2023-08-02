using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dart : Weapon
{
    private Rigidbody2D _rgBody2D;
    private Rigidbody2D rgBody2D
    {
        get
        {
            if(_rgBody2D==null) _rgBody2D = GetComponent<Rigidbody2D>();
            return _rgBody2D;
        }
    }
    //Boomerang effect
    protected bool isFiring;
    private Vector2 direction;
    private float startThrowSpeed = 9.8f;
    private float currentThrowSpeed;
    private float acceleration = 9.8f;
    private Vector2 offSet = new Vector2(0,0.1f); // to make it not straight boomerang
    private Transform player;
    //Rotation
    private float timeCounter = 0;
    private float rotationSpeed = 10f;

    public override void SetDame(int level)
    {
        DEFAULT_DAME = 5;
        dameStep = 1;
        base.SetDame(level);
    }
    private void FixedUpdate()
    {
        if (isFiring)
        {
            timeCounter += Time.fixedDeltaTime;
            Throw();
            Rotation();
            CheckOutOfScreen();
        }
    }
    public void SetPlayer(Transform pl)
    {
        player = pl;
    }
    public void Init(Vector2 targetDirection)
    {
        timeCounter = 0;
        direction = (targetDirection - (Vector2)transform.position).normalized;
        isFiring = true;
    }
    private void Throw()
    {
        currentThrowSpeed = startThrowSpeed - acceleration * timeCounter;
        rgBody2D.velocity = currentThrowSpeed * (direction - offSet);
    }
    private void CheckOutOfScreen()
    {
        if (currentThrowSpeed < 0)
        {
            if (transform.position.x >= player.transform.position.x + MAX_X_DISTANCE_FROM_PLAYER && direction.x < 0||
                transform.position.x <= player.transform.position.x - MAX_X_DISTANCE_FROM_PLAYER && direction.x > 0 ||
                transform.position.y >= player.transform.position.y + MAX_Y_DISTANCE_FROM_PLAYER && direction.y < 0 ||
                transform.position.y <= player.transform.position.y - MAX_Y_DISTANCE_FROM_PLAYER && direction.y > 0)
            {
                isFiring = false;
                ReturnToPool();
            }
        }
    }
    private void Rotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg*(timeCounter * rotationSpeed)));
    }
}
