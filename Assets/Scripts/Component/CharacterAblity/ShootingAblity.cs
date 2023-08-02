using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAblity : Ablities // Passive Ablities
{
    [SerializeField] protected ItemPoolManager itemPoolManager;
    Vector2 startPoint;
    Vector2 endPoint;
    protected Camera cam;
    protected int numberBulletStart ;
    protected float timeCountdown;
    protected float timeOffset;

    private int numberBullet;

    protected virtual void Awake()
    {
        cam = Camera.main;
        numberBulletStart = 3;
        timeCountdown = 1f;
        timeOffset = 0.1f;
        isPassiveAblity = PlayerPrefs.GetInt(PlayerPrefsString.characterString,0)==0;
    }
    private Coroutine corShooting;
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
            numberBullet = numberBulletStart + (level - 1);
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
    
    private bool isShooting = true;
    private IEnumerator IEShooting()
    {
        isShooting = true;
        for (int i = 0; i < numberBullet; i++)
        {
            startPoint = transform.position;
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Shoot(startPoint, endPoint);
            yield return new WaitForSeconds(timeOffset);
        }
        yield return new WaitForSeconds(timeCountdown);
        isShooting = false;
    }
    protected virtual void Shoot(Vector2 startPoint, Vector2 endPoint)
    {
        Bullet bullet = (Bullet)itemPoolManager.GetPrefabInstance();
        bullet.transform.position = transform.position;
        bullet.CalculatePosition(startPoint, endPoint);
        bullet.SetDame(level);
        DucSoundManager.Instance.PlaySoundFx("sfx_projectile");
    }
}
