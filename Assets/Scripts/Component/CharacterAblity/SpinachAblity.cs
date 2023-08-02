using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinachAblity : Ablities
{
    private readonly int DEFAULT_DAMAGE = 1;
    public static float dameBonus;
    private float bonusEachLevel = 0.1f;
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
        dameBonus = DEFAULT_DAMAGE + bonusEachLevel * (level - 1);
    }
}
