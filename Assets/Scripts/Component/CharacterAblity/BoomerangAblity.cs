using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAblity : ShootingAblity
{
    protected override void Awake()
    {
        cam = Camera.main; 
        numberBulletStart = 1;
        timeCountdown = 2f;
        timeOffset = 0.1f;
        isPassiveAblity = PlayerPrefs.GetInt(PlayerPrefsString.characterString, 0) == 2;
    }
    protected override void Shoot(Vector2 startPoint, Vector2 endPoint)
    {
        Dart bullet = (Dart)itemPoolManager.GetPrefabInstance();
        bullet.transform.position = transform.position;
        bullet.Init(endPoint);
        bullet.SetPlayer(transform.parent.parent);
        bullet.SetDame(level);
        DucSoundManager.Instance.PlaySoundFx("sfx_projectile");
    }
}
