using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : ItemPool
{
    public int level;
    protected float DEFAULT_DAME;
    protected float dame;
    protected float dameStep;

    protected readonly float MAX_X_DISTANCE_FROM_PLAYER = 8.575f; //In Screen Bound
    protected readonly float MAX_Y_DISTANCE_FROM_PLAYER = 4.685f; //In Screen Bound

    public virtual void SetDame(int level)
    {
        this.level = level;
        dame = DEFAULT_DAME + (level - 1) * dameStep;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        CollideEnemy(collision);
    }
    protected void CollideEnemy(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")) || collision.gameObject.layer.Equals(LayerMask.NameToLayer("EnemyGhost")))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.GetDame((int)(dame * SpinachAblity.dameBonus));
            DoAfterCollide();
        }
    }
    protected virtual void DoAfterCollide()
    {

    }
}
