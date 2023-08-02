using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    private int hp;
    private ProtectorType type;
    [SerializeField] private Guard guardPrefab;
    private ItemPoolManager itemPoolManager;
    private void Start()
    {
        InitType(ProtectorType.TRAINING_CAMP);
    }
    public void InitType(ProtectorType type)
    {
        this.type = type;
        StartAction();
    }
    private void StartAction()
    {
        switch (type)
        {
            case ProtectorType.TRAINING_CAMP:
            {
                DoTrainingCamp();
                break;
            }
            default: break;
        }
    }
    private List<Guard> guardList = new List<Guard>();
    private void DoTrainingCamp()
    {
        itemPoolManager = gameObject.AddComponent<ItemPoolManager>();
        itemPoolManager.prefab = guardPrefab;
        itemPoolManager.maxItemInPool = 3;
        InvokeRepeating("SpawnGuard", 5, 10);
    }
    private void SpawnGuard()
    {
        Guard guard = (Guard)itemPoolManager.GetPrefabInstance();
        guard.transform.position = GetRandomTargetPosition();
        guard.StartDetectTarget();
    }
    private Vector2 GetRandomTargetPosition()
    {
        int randomAngle = Random.Range(0, 360);
        Vector2 randomVector = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        return (Vector2)transform.position + 2 * randomVector;
    }
}
public enum ProtectorType
{
    TRAINING_CAMP,
    BELVEDERE,
    MAGIC_TOWER,
    LIGHTNING_TOWER,
    CANON_TOWER,
}
