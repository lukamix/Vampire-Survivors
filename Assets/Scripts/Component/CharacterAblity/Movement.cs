using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Ablities
{
    float horizontal;
    float vertical;
    private Vector2 moveDirection;
    private readonly float DEFAULT_SPEED = 3;
    private float speed = 3;
    private float bonuEachLevel = 0.1f;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField]
    private EntityAnimation playerAnimation;
    private void Awake()
    {
        isPassiveAblity = true;
    }
    public override void DoAblities()
    {
        if (isStartDoing)
        {
            if (level <= 0 && isPassiveAblity) level = 1;
            isStartDoing = false;
        }
        else
        {
            level++;
        }
        if (level <= 0) return;
        speed = DEFAULT_SPEED * (1 + bonuEachLevel * (level - 1));
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontal, vertical);
        playerAnimation.AnimationUpdate(moveDirection);
        rigidBody2D.velocity = speed * moveDirection;
    }
}
