using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool isIdle = true;
    private bool isMove = false;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            return _animator;
        }
    }
    public void AnimationUpdate(Vector2 move_direction)
    {
        if (move_direction.x == 0)
        {
            if (!isIdle)
            {
                isIdle = true;
                isMove = false;
                SetIdleAnimation();
            }
        }
        else
        {
            if (!isMove)
            {
                isIdle = false;
                isMove = true;
                SetMovingAnimation();
            }
            if (move_direction.x < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
    private void SetIdleAnimation()
    {
        animator.Play("Idle");
    }
    private void SetMovingAnimation()
    {
        animator.Play("Move");
    }
    public void PlayHitEffect()
    {
        animator.Play("Hit");
    }
    public void ReturnAnimation()
    {
        if (isMove)
        {
            SetMovingAnimation();
        }
        else
        {
            SetIdleAnimation();
        }
    }
}