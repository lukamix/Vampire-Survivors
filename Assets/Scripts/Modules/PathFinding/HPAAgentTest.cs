using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPAAgentTest : MonoBehaviour
{
    private GridTile currentPos;
    private LinkedListNode<Edge> currentEdge = null;

    private Rigidbody2D _rigidBody2D;
    public Rigidbody2D rgbd2D
    {
        get
        {
            if (_rigidBody2D == null) _rigidBody2D = GetComponent<Rigidbody2D>();
            return _rigidBody2D;
        }
    }
    [SerializeField]
    private HPAMap map;
    public void SetDestination(GridTile pos)
    {
        currentPos = new GridTile(new Vector3(transform.position.y,transform.position.x,0));
        LinkedList<Edge> path = map.GetPath(currentPos, pos);
        currentEdge = path.First;
        GridTile curr = currentEdge.Value.end.pos;
        targetPosition = new Vector2(curr.y, curr.x);
        isReachTarget = false;
    }
    private void Awake()
    {
        StartCoroutine(FindPath());
    }
    private IEnumerator FindPath()
    {
        yield return new WaitForSeconds(1);
        DaiBang target = GameObject.Find("Princess").GetComponent<DaiBang>();
        SetDestination(new GridTile(target.transform.position));
    }
    private float distanceThreshHold = 0.1f;
    private float speed = 1f;
    private bool isReachTarget = true;
    public Vector2 targetPosition;
    public Vector2 direction;
    private void FixedUpdate()
    {
        if (!isReachTarget)
        {
            if (currentEdge!=null)
            {
                float distance = Vector2.Distance(transform.position, targetPosition);
                if (distance < distanceThreshHold)
                {
                    currentEdge = currentEdge.Next;
                    GridTile curr = currentEdge.Value.end.pos;
                    targetPosition = new Vector2( curr.y, curr.x);
                }
            }
            // Set velocity
            direction = (targetPosition - (Vector2)transform.position).normalized;
            rgbd2D.velocity = speed * direction;
        }
    }
}
