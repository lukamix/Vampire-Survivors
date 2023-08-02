using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiBang : MonoBehaviour
{
    private float currentHPPercent = 100;
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            return _animator;
        }
    }
    [SerializeField] private RectTransform HPBar;
    public void GetDamage(float dame)
    {
        PlayHitEffect();
        if (currentHPPercent > dame)
        {
            currentHPPercent -= dame;
        }
        else
        {
            currentHPPercent = 0;
        }
        HPBar.sizeDelta = currentHPPercent / 100 * Vector2.right;
        if (currentHPPercent <= 0)
        {
            Observer.Instance.Notify(ObserverKey.GameOver);
        }
    }
    public void IncreaseHealth(int health)
    {
        GameManager.Instance.damageNumber.Spawn((Vector2)transform.position + Vector2.up, health, Color.blue);
        if (currentHPPercent <= 100-health)
        {
            currentHPPercent += health;
        }
        else
        {
            currentHPPercent = 100;
        }
    }
    public void PlayHitEffect()
    {
        animator.Play("Hit");
    }
    public void ReturnAnimation()
    {
        animator.Play("Idle");
    }
}
