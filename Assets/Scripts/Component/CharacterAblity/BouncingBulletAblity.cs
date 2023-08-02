using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBulletAblity : Ablities
{
    [SerializeField] private ItemPoolManager itemPoolManager;

    private Coroutine corShooting;

    private float timeCountdown = 5f;
    private float timeOffset = 0.1f;

    public int numberBullet = 4;
    private bool isShooting = true;
    private void FixedUpdate()
    {
        if (!isShooting)
        {
            corShooting = StartCoroutine(IEShooting());
        }
    }
    public override void DoAblities()
    {
        if (isStartDoing)
        {
            if (level <= 0 && isPassiveAblity) level = 1;
            numberBullet = 2 + level;
            isStartDoing = false;
        }
        else
        {
            level++;
            numberBullet++;
        }
        if (level <= 0) return;
        if (corShooting != null)
        {
            StopCoroutine(corShooting);
        }
        isShooting = false;
    }
    private IEnumerator IEShooting()
    {
        isShooting = true;
        for (int i = 0; i < numberBullet; i++)
        {
            Shoot(i);
            yield return new WaitForSeconds(timeOffset);
        }
        yield return new WaitForSeconds(timeCountdown);
        isShooting = false;
    }
    private void Shoot(int i)
    {
        BouncingBullet bullet = (BouncingBullet)itemPoolManager.GetPrefabInstance();
        bullet.transform.position = transform.position;
        bullet.SetDirection(i);
        bullet.SetPlayer(transform.parent.parent);
        bullet.SetDame(level);
        DucSoundManager.Instance.PlaySoundFx("sfx_projectile");
    }
}