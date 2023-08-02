using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterGlass : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // do nothing.
    }
    protected override void CheckLand()
    {
        if (currentFiringTime > totalTime)
        {
            isFiring = false;
            currentFiringTime = 0;
            Observer.Instance.Notify(ObserverKey.HolyWaterGlassBreak, this);
        }
    }
}