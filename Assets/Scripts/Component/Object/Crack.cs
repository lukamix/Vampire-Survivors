using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crack : Weapon
{
    public override void SetDame(int level)
    {
        DEFAULT_DAME = 10;
        dameStep = 5;
        base.SetDame(level);
    }
}
