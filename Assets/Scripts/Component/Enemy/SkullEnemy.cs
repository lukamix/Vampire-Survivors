using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : Enemy
{
    public override void SetDefaultProperties()
    {
        DEFAULT_ATTACKSPEED = 0.5f;
        DEFAULT_DAME = 1.5f;
        DEFAULT_SPEED = 0.6f;
        DEFAULT_HP = 10;
        DEFAULT_COIN_DROP = 3;
    }

}
