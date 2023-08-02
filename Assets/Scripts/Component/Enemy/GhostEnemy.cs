using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostEnemy : Enemy
{
    public override void SetDefaultProperties()
    {
        DEFAULT_ATTACKSPEED = 0.5f;
        DEFAULT_DAME = 2;
        DEFAULT_SPEED = 0.5f;
        DEFAULT_HP = 5;
        DEFAULT_COIN_DROP = 2;
    }
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
        StartCoroutine(IESurvivor());
    }
    private IEnumerator IESurvivor()
    {
        yield return new WaitForSeconds(5);
        ReturnToPool();
    }
    protected override void FixedUpdate()
    {
        rgbd2D.velocity = speed * direction;
    }
}
