using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : Enemy
{
    public override void SetDefaultProperties()
    {
        DEFAULT_ATTACKSPEED = 0.5f;
        DEFAULT_DAME = 1;
        DEFAULT_SPEED = 0.7f;
        DEFAULT_HP = 5;
        DEFAULT_COIN_DROP = 2;
    }
}
