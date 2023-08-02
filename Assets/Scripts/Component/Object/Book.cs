using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Weapon
{
    public override void SetDame(int level)
    {
        DEFAULT_DAME = 10;
        dameStep = 5;
        base.SetDame(level);
    }
}
