using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorSupport : MonoBehaviour
{
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            return _animator;
        }
    }
    private void GetHitEffect()
    {
        animator.Play("hit");
    }
    public void TurnToIdle()
    {
        animator.Play("idle");
    }
}
