using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public override void SetDefaultProperties()
    {
        DEFAULT_ATTACKSPEED = 0.25f;
        DEFAULT_DAME = 10f;
        DEFAULT_SPEED = 0.4f;
        DEFAULT_HP = 400;
        DEFAULT_COIN_DROP = 50;
    }
    private void SpeedUp(int exp)
    {
        speed = exp * DEFAULT_SPEED;
        attackSpeed = exp * DEFAULT_ATTACKSPEED;
    }
    public override void GetDame(int dame)
    {
        base.GetDame(dame);
        if(currentHP < DEFAULT_HP /2)
        {
            SpeedUp(2);
        }
    }
    public override void Dead()
    {
        Observer.Instance.Notify(ObserverKey.BossTime,false);
        Chest chest = (Chest)ItemManager.Instance.chestPool.GetPrefabInstance();
        chest.transform.position = transform.position;
        Destroy(gameObject);
    }
}
