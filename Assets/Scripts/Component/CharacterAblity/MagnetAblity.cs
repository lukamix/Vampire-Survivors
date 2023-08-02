using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetAblity : Ablities
{
    [SerializeField] private CircleCollider2D col;
    private readonly int DEFAULT_RADIUS = 1;
    private int radius;
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
        radius = DEFAULT_RADIUS + (level - 1);
    }
}
