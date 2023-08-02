using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Guard : ItemPool
{
    [SerializeField] private RectTransform hpBar;
    private float currentHPPercent=1;
    private Enemy target;
    private int dame = 10;
    private Vector2 targetPosition;
    private float speed = 1f;
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            return _animator;
        }
    }
    private Rigidbody2D _rigidBody2D;
    public Rigidbody2D rgbd2D
    {
        get
        {
            if (_rigidBody2D == null) _rigidBody2D = GetComponent<Rigidbody2D>();
            return _rigidBody2D;
        }
    }
    private void SetHPBar()
    {
        hpBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHPPercent);
    }
    public void GetDamage()
    {
        currentHPPercent -= 0.5f;
        if (currentHPPercent <= 0)
        {
            ReturnToPool();
        }
        SetHPBar();
    }
    private bool isReachTarget = true;
    private float distanceThreshHold = 2f;
    private bool isMoving = false;
    private void FixedUpdate()
    {
        if (!isReachTarget)
        {
            if (!isMoving)
            {
                animator.Play("Walk");
            }
            if (target == null || !target.gameObject.activeSelf)
            {
                target = null;
                targetPosition = startPosition;
            }
            else
            {
                targetPosition = target.transform.position;
            }
            float distance = Vector2.Distance(transform.position, targetPosition);
            if (distance < distanceThreshHold)
            {
                animator.Play("Idle");
                if(target!=null && target.gameObject.activeSelf)
                {
                    target.ChangeTarget(transform);
                }
                isReachTarget = true;
            }
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            transform.localScale = direction.x>0?Vector3.one:new Vector3(-1,1,1);
            rgbd2D.velocity = speed * direction;
        }
        else
        {
            Attack();
        }
    }
    private Vector2 startPosition;
    public void StartDetectTarget()
    {
        startPosition = transform.position;
        InvokeRepeating("DetectTarget", 0, 3);
    }
    private int targetRange = 5;
    private void DetectTarget()
    {
        float distanceToTarget = 0;
        if (target != null)
        {
            distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        }
        if(target==null|| !target.gameObject.activeSelf || (target!=null && distanceToTarget<targetRange))
        {
            target = null;
            int enemyLayer = 1 << 8;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, targetRange, enemyLayer);
            if (enemies.Length > 0)
            {
                float distance = Vector2.Distance(transform.position, enemies[0].transform.position);
                int index = 0;
                for (int i = 0; i < enemies.Length; i++)
                {
                    float newDistance = Vector2.Distance(transform.position, enemies[i].transform.position);
                    if (distance > newDistance)
                    {
                        distance = newDistance;
                        index = i;
                    }
                }
                target = enemies[index].GetComponent<Enemy>();
                isReachTarget = false;
            }
        }
    }
    private bool isWaitingForAttack = false;
    private void Attack()
    {
        if (target != null && target.gameObject.activeSelf)
        {
            if (!isWaitingForAttack)
            {
                StartCoroutine(IEAttack());
            }
        }
        else
        {
            target = null;
        }
    }
    private IEnumerator IEAttack()
    {
        isWaitingForAttack = true;
        animator.Play("Attack");
        yield return new WaitForSeconds(0.5f);
        animator.Play("Walk");
        if (target != null && target.gameObject.activeSelf)
        {
            target.GetDame(dame);
        }
        isWaitingForAttack = false;
    }
}
