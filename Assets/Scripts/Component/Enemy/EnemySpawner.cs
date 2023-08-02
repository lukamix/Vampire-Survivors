using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> EnemyPrefab = new List<Enemy>();
    [SerializeField] private List<Boss> BossPrefab = new List<Boss>();
    [SerializeField] private HPAMap hpaMap;
    [HideInInspector] public DaiBang target;
    [HideInInspector] public Transform player;
    private List<ItemPoolManager> enemyPoolManager = new List<ItemPoolManager>();

    //Spawn Ghost Enemy
    [SerializeField] private ItemPoolManager ghostEnemyPoolManager ;

    int[,] mapArray;
    int mapSizeX, mapSizeY;
    int kingZoneSize;
    private List<Enemy> listSpawnedEnemy = new List<Enemy>();
    public void Init(int[,] mapArray, int kingZoneSize, int mapSizeX,int mapSizeY)
    {
        InitEnemyPool();
        DaiBang target = GameObject.Find("Princess").GetComponent<DaiBang>();
        this.target = target;
        this.kingZoneSize = kingZoneSize;
        this.mapArray = mapArray;
        this.mapSizeX = mapSizeX;
        this.mapSizeY = mapSizeY;
    }
    private void InitEnemyPool()
    {
        for(int i = 0; i < EnemyPrefab.Count; i++)
        {
            ItemPoolManager newPool = gameObject.AddComponent<ItemPoolManager>();
            newPool.prefab = EnemyPrefab[i];
            newPool.maxItemInPool = 1000;
            enemyPoolManager.Add(newPool);
        }
    }
    public void SpawnGhostEnemy()
    {
        float enemyDistance = 0.5f;
        int corner = Random.Range(0, 4);
        int xSign = corner % 2 == 0 ? 1 : -1;
        int ySign = corner % 2 == 1 ? 1 : -1;
        Vector2 direction = Vector2.zero;

        for(int i = 0; i < 16; i++)
        {
            GhostEnemy enemy = (GhostEnemy)ghostEnemyPoolManager.GetPrefabInstance();
            if (!listSpawnedEnemy.Contains(enemy))
            {
                listSpawnedEnemy.Add(enemy);
            }
            enemy.UpdateProperties();
            enemyDistance = Random.Range(0.5f, 1);
            enemy.transform.position = player.position + new Vector3(xSign * 9,ySign * 5.5f,0)+ enemyDistance * new Vector3((int)(i/4),i%4,0);
            enemy.transform.localScale = new Vector3(2, 2, 2);
           enemy.player = player;
            if(direction.x==0 && direction.y == 0)
            {
                direction = (player.position - enemy.transform.position);
            }
            enemy.SetDirection(direction);
            enemy.transform.rotation = Quaternion.Euler(new Vector3(0, direction.x > 0 ? 180 : 0, 0));
            enemy.mapArray = mapArray;
        }
    }
    public void SpawnBoss(int bossId,int targetRange)
    {
        Vector2 position = GetSpawnablePosition();
        GetSpawnAblePointsList(mapArray, target.transform.position, targetRange);
        Boss boss = Instantiate(BossPrefab[bossId]);
        boss.UpdateProperties();
        boss.transform.position = position;
        boss.targetTransform = target;
        boss.player = player;
        boss.hPAMap = hpaMap;
        boss.target = GetTargetArroundIndex(boss);
        boss.mapArray = mapArray;
        boss.MoveToTarget();
    }
    public void SpawnEnemy(int number,int targetRange)
    {
        GetSpawnAblePointsList(mapArray, target.transform.position, targetRange);
        for (int i = 0; i < number; i++)
        {
            StartCoroutine(IESpawnEnemy());
        }
    }
    public void ClearAllEnemy()
    {
        foreach(Enemy e in listSpawnedEnemy)
        {
            if (e.gameObject.activeSelf)
            {
                e.Dead();
            }
        }
    }
    private IEnumerator IESpawnEnemy()
    {
        Vector2 position = GetSpawnablePosition();
        int randomValue = Random.Range(0, EnemyPrefab.Count);
        Enemy enemy = (Enemy)enemyPoolManager[randomValue].GetPrefabInstance();
        if (!listSpawnedEnemy.Contains(enemy))
        {
            listSpawnedEnemy.Add(enemy);
        }
        enemy.UpdateProperties();
        enemy.transform.position = position;
        enemy.targetTransform = target;
        enemy.player = player;
        enemy.hPAMap = hpaMap;
        enemy.target = GetTargetArroundIndex(enemy);
        enemy.mapArray = mapArray;
        enemy.MoveToTarget();
        yield return new WaitForSeconds(0.5f);
    }
    private Vector2 GetTargetArroundIndex(Enemy enemy)
    {
        Vector2 direction = (target.transform.position - enemy.transform.position).normalized;
        Vector2 targetPosition = (Vector2)target.transform.position - direction * (kingZoneSize - 1);
        targetPosition = new Vector2((int)targetPosition.x, (int)targetPosition.y);
        Vector2 targetIndex = targetPosition;
        if (mapArray[(int)targetIndex.x, (int)targetIndex.y] == -1)
        {
            return GetNeighbour(targetPosition,targetIndex);
        }
        return targetPosition;
    }
    private Vector2 GetNeighbour(Vector2 targetPosition,Vector2 targetIndex)
    {
        int offSet = 1;
        if (mapArray[(int)targetIndex.x + offSet, (int)targetIndex.y] == 0)
        {
            return targetPosition + offSet * Vector2.right;
        }
        if (mapArray[(int)targetIndex.x - offSet, (int)targetIndex.y] == 0)
        {
            return targetPosition + offSet * Vector2.left;
        }
        if (mapArray[(int)targetIndex.x, (int)targetIndex.y + offSet] == 0)
        {
            return targetPosition + offSet * Vector2.up;
        }
        if (mapArray[(int)targetIndex.x, (int)targetIndex.y - offSet] == 0)
        {
            return targetPosition + offSet * Vector2.down;
        }
        if (mapArray[(int)targetIndex.x + offSet, (int)targetIndex.y + offSet] == 0)
        {
            return targetPosition + offSet * Vector2.one;
        }
        if (mapArray[(int)targetIndex.x - offSet, (int)targetIndex.y - offSet] == 0)
        {
            return targetPosition - offSet * Vector2.one;
        }
        if (mapArray[(int)targetIndex.x + offSet, (int)targetIndex.y - offSet] == 0)
        {
            return targetPosition + offSet * new Vector2(1,-1);
        }
        if (mapArray[(int)targetIndex.x - offSet, (int)targetIndex.y + offSet] == 0)
        {
            return targetPosition - offSet * new Vector2(1, -1);
        }
        return targetPosition;
    }
    private List<Vector2> spawnablePoints = new List<Vector2>();
    private void GetSpawnAblePointsList(int[,] mapArray,Vector2 target, int targetRange)
    {
        int targetX = (int)(target.x);
        int targetY = (int)(target.y);
        for (int i = -targetRange; i < targetRange; i++)
        {
            if (targetX + i < 0) continue;
            if (targetX + i >= mapSizeX) break;
            for (int j = -targetRange; j < targetRange; j++)
            {
                if (targetY + j < 0) continue;
                if (targetY + j >= mapSizeY) break;
                if (Mathf.Sqrt(i*i + j*j) < (targetRange / Mathf.Sqrt(2))) continue;
                if (mapArray[targetX + i, targetY + j] == 0)
                {
                    spawnablePoints.Add(new Vector2((targetX + i), (targetY + j)));
                }
            }
        }
    }
    private Vector2 GetSpawnablePosition()
    {
        int rdvl = Random.Range(0, spawnablePoints.Count);
        return spawnablePoints[rdvl];
    }
}
