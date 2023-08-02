using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : ItemPool
{
    [HideInInspector]
    public Vector2 target;
    [HideInInspector]
    public DaiBang targetTransform;
    //use HPA Star
    [HideInInspector]
    public HPAMap hPAMap;

    [HideInInspector] public Transform player;

    private Transform tempTransformTarget;
    private float attackRange = 2; //Throw Bullet
    public int[,] mapArray;
    private float distanceThreshHold = 0.1f;
    private float distanceOffset = 0.01f;

    protected float speed;
    protected float attackSpeed; //attack per sec
    protected float currentHP;
    protected float dame;
    protected int maxCoinDrop;

    protected float DEFAULT_SPEED;
    protected float DEFAULT_ATTACKSPEED;
    protected float DEFAULT_HP;
    protected float DEFAULT_DAME;
    protected int DEFAULT_COIN_DROP;

    private bool isReachTarget =true;
    private bool isGhost =false;
    public Vector2 targetPosition;
    public Vector2 direction;
    private Rigidbody2D _rigidBody2D;
    public Rigidbody2D rgbd2D
    {
        get
        {
            if (_rigidBody2D == null) _rigidBody2D = GetComponent<Rigidbody2D>();
            return _rigidBody2D;
        }
    }
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
    public virtual void GetDame(int dame)
    {
        DucSoundManager.Instance.PlaySoundFx("sfx_enemyHit");
        KnockBack();
        GetHitEffect();
        if (currentHP <= dame)
        {
            GameManager.Instance.damageNumber.Spawn((Vector2)transform.position + Vector2.up, currentHP, Color.red);
            Dead();
        }
        else
        {
            GameManager.Instance.damageNumber.Spawn((Vector2)transform.position + Vector2.up, dame, Color.red);
            currentHP -= dame;
        }
    }
    private void KnockBack()
    {
        Vector2 forceDirection = (transform.position - player.transform.position).normalized;
        rgbd2D.AddForce(150*forceDirection);
    }
    public virtual void SetDefaultProperties()
    {

    }
    private float currentWaitingTime = 0f;
    private float timeOffset = 1f;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (currentWaitingTime <= 0)
        {
            OnCollide(collision);
            currentWaitingTime = timeOffset;
        }
        else
        {
            currentWaitingTime -= Time.fixedDeltaTime;
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollide(collision);
    }
    private void OnCollide(Collision2D collision)
    {
        currentWaitingTime = timeOffset;
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Character")))
        {
            Attribute att = collision.gameObject.GetComponent<Attribute>();
            att.GetDame(dame);
        }
        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("King")))
        {
            DaiBang db = collision.gameObject.GetComponent<DaiBang>();
            db.GetDamage(dame);
        }
    }
    public void UpdateProperties()
    {
        SetDefaultProperties();
        int exp = (int)(GameManager.Instance.gameTime / 60);
        currentHP = DEFAULT_HP * (1 + exp);
        attackSpeed = DEFAULT_ATTACKSPEED;
        speed = DEFAULT_SPEED;
        dame = DEFAULT_DAME * (1 + exp);
        maxCoinDrop = DEFAULT_COIN_DROP;
    }
    public virtual void Dead()
    {
        bool dropCoin = (Random.Range(0, 100) < 10);
        bool dropFood = (Random.Range(0, 100) < 1);
        bool dropChest = (Random.Range(0, 1000) < 5);
        if (dropChest)
        {
            Chest chest = (Chest)ItemManager.Instance.chestPool.GetPrefabInstance();
            chest.transform.position = transform.position;
        }
        else if (dropFood)
        {
            Food food = (Food)ItemManager.Instance.foodPool.GetPrefabInstance();
            food.healh = 50;
            food.transform.position = transform.position;
        }
        else if (dropCoin)
        {
            Coin coin = (Coin)ItemManager.Instance.coinPool.GetPrefabInstance();
            coin.coinCount = Random.Range(1, maxCoinDrop+1);
            coin.transform.position = transform.position;
        }
        else
        {
            EXP exp = (EXP)ItemManager.Instance.itemPool.GetPrefabInstance();
            exp.transform.position = transform.position;
            int randomType = Random.Range(0, 1000);
            EXPType type = EXPType.GREEN;
            if (randomType > 200)
            {
                type = EXPType.GREEN;
            }
            else if (randomType > 40)
            {
                type = EXPType.BLUE;
            }
            else if (randomType > 8)
            {
                type = EXPType.Yellow;
            }
            else
            {
                type = EXPType.RED;
            }
            exp.SetEXPType(type);
        }
        Observer.Instance.Notify(ObserverKey.EnemyDead);
        ReturnToPool();
    }
    //Path finding
    private GridTile currentPos;
    private LinkedListNode<Edge> currentEdge = null;
    public virtual void MoveToTarget()
    {
        currentPos = new GridTile(new Vector3(transform.position.y, transform.position.x, 0));
        LinkedList<Edge> path = hPAMap.GetPath(currentPos, new GridTile(new Vector3(target.y, target.x, 0)));
        currentEdge = path.First;
        GridTile curr = currentEdge.Value.end.pos; // vị trí target là vị trí cuối của cạnh
        targetPosition = new Vector2(curr.y, curr.x);
        isReachTarget = false;
    }

    //Move
    private Vector2 lastPosition;
    private bool check = false;
    private void CheckStuck()
    {
        StartCoroutine(IECheckStuck());
    }
    private IEnumerator IECheckStuck()
    {
        check = true;
        yield return new WaitForSeconds(1);
        if(((Vector2)transform.position - lastPosition).magnitude< distanceOffset)
        {
            if (currentEdge!= null)
            {
                if (currentEdge.Next != null)
                {
                    currentEdge = currentEdge.Next;
                    GridTile curr = currentEdge.Value.end.pos;
                    targetPosition = new Vector2(curr.y, curr.x);
                }
            }
            gameObject.layer = 11;
            isGhost = true;
            TurnToGhost();
        }
        check = false;
    }
    protected virtual void FixedUpdate()
    {
        if (!isReachTarget)
        {
            if (!check) { CheckStuck(); }
            if (currentEdge != null)
            {
                float distance = Vector2.Distance(transform.position, targetPosition);
                if (distance < distanceThreshHold)
                {
                    currentEdge = currentEdge.Next;
                    if (currentEdge != null)
                    {
                        GridTile curr = currentEdge.Value.end.pos;
                        targetPosition = new Vector2(curr.y, curr.x);
                    }
                }
            }
            else // Free Moving To Target
            {
               
                if (tempTransformTarget!=null && tempTransformTarget.gameObject.activeSelf)
                {
                    targetPosition = tempTransformTarget.transform.position;
                }
                else
                {
                    if(tempTransformTarget != null)
                    {
                        tempTransformTarget = null;
                    }
                    //Move to princess
                    targetPosition = targetTransform.transform.position;
                }
                float distance = Vector2.Distance(transform.position, targetPosition);
                if (distance < attackRange)
                {
                    isReachTarget = true;
                }
            }
            // Set velocity
            direction = (targetPosition - (Vector2)transform.position).normalized;
            if(direction.x>0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            rgbd2D.velocity = speed * direction;
            lastPosition = transform.position;
        }
        else
        {
            rgbd2D.velocity = Vector2.zero;
            if (tempTransformTarget != null && tempTransformTarget.gameObject.activeSelf)
            {
                AttackTarget(tempTransformTarget);
            }
            else
            {
                if (tempTransformTarget != null)
                {
                    tempTransformTarget = null;
                    isReachTarget = false;
                    targetPosition = targetTransform.transform.position;
                }
                else
                {
                    AttackTarget(targetTransform.transform);
                }
            }
        }
    }
    public void ChangeTarget(Transform target)
    {
        tempTransformTarget = target;
    }
    //Attack
    private bool isWaitingForNextAttack = false;
    private IEnumerator IEAttackTarget(Transform target)
    {
        isWaitingForNextAttack = true;
        EnemyBullet bullet = (EnemyBullet)EnemyBulletManager.Instance.itemPoolManager.GetPrefabInstance();
        bullet.transform.position = transform.position;
        bullet.transform.localScale = 0.5f * Vector2.one;
        bullet.SetDame(dame);
        bullet.SetTarget(target);
        yield return new WaitForSeconds(1f/attackSpeed);
        isWaitingForNextAttack = false;
    }
    private void AttackTarget(Transform target)
    {
        if (!isWaitingForNextAttack)
        {
            StartCoroutine(IEAttackTarget(target));
        }
    }
    //Animation
    private void GetHitEffect()
    {
        animator.Play("hit");
    }
    public void TurnToIdle()
    {
        if (!isGhost)
            animator.Play("idle");
        else
        {
            TurnToGhost();
        }
    }
    public void TurnToGhost()
    {
        animator.Play("ghost");
    }
}