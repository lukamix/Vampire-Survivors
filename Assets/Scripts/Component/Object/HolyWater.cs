using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWater : Weapon
{
    private float timeOffset = 1f;
    private float currentWaitingTime = 0f;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        CollideEnemy(collision);
        currentWaitingTime = timeOffset;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentWaitingTime <= 0)
        {
            CollideEnemy(collision);
            currentWaitingTime = timeOffset;
        }
        else
        {
            currentWaitingTime -= Time.fixedDeltaTime;
        }
    }

    public override void SetDame(int level)
    {
        DEFAULT_DAME = 10;
        dameStep = 5;
        base.SetDame(level);
    }
}
