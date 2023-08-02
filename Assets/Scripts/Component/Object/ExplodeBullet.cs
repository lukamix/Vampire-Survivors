using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBullet : Bullet
{
    protected override void CheckLand()
    {
        if (currentFiringTime > totalTime)
        {
            Explode();
        }
    }
    protected override void DoAfterCollide()
    {
        base.DoAfterCollide();
        Explode();
    }
    private void Explode()
    {
        isFiring = false;
        currentFiringTime = 0;
        Destroy(gameObject);
    }
}
