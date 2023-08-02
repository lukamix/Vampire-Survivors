using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [HideInInspector]public EnemySpawner enemySpawner;
    [HideInInspector] public Transform player;
    private int numberEnemySpawn = 2;
    private int stepNumberEnemy = 1;
    private int maxEnemySpawn = 50;

    int maxTargetRange = 15;

    private int startSpawnTime = 1; //Since Start Game;
    private int timeSpawnOffset = 15; // 2 lan lien tiep cach nhau 2s

    private int startSpawnGhostEnemyTime = 30;
    private int timeSpawnGhostEnemyOffset = 30;
    private int hardLevel;
    public void EnemiesManagerStart(MapGenerator mapGenerator)
    {
        Observer.Instance.AddObserver(ObserverKey.BossTime, DoOnBossTime);
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.Init(mapGenerator.map,mapGenerator.kingZoneSize, mapGenerator.mapSizeX, mapGenerator.mapSizeY);
        enemySpawner.player = player;
        RepeatingSpawnEnemy();
        RepeatingSpawnGhostEnemy();
        SetHardLevel();
    }
    public void ClearAllEnemy()
    {
        CancelInvoke();
        enemySpawner.ClearAllEnemy();
    }
    private void RepeatingSpawnGhostEnemy()
    {
        InvokeRepeating("SpawnGhostEnemy", startSpawnGhostEnemyTime, timeSpawnGhostEnemyOffset);
    }
    private void SpawnGhostEnemy()
    {
        enemySpawner.SpawnGhostEnemy();
    }
    private void RepeatingSpawnEnemy()
    {
        InvokeRepeating("AutoSpawnEnemy", startSpawnTime, timeSpawnOffset);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.BossTime, DoOnBossTime);
    }
    private void SetHardLevel()
    {
        hardLevel = PlayerPrefs.GetInt(PlayerPrefsString.levelString, 0);
        if (hardLevel == 0)
        {
            SetEasyMode();
        }
        else if(hardLevel == 1)
        {
            SetNormalMode();
        }
        else if (hardLevel == 2)
        {
            SetHardMode();
        }
    }
    private void SetEasyMode()
    {
        numberEnemySpawn = 2;
        stepNumberEnemy = 2;
        maxEnemySpawn = 50;
    }
    private void SetNormalMode()
    {
        numberEnemySpawn = 3;
        stepNumberEnemy = 3;
        maxEnemySpawn = 75;
    }
    private void SetHardMode()
    {
        numberEnemySpawn = 5;
        stepNumberEnemy = 4;
        maxEnemySpawn = 100;
    }
    private void DoOnBossTime(object data)
    {
        bool _isBossTime = (bool)data;
        if (_isBossTime)
        {
            SpawnBoss(0);
        }
        else
        {
            RepeatingSpawnEnemy();
        }
    }
    private void SpawnBoss(int id)
    {
        enemySpawner.SpawnBoss(id, 10);
        CancelInvoke();
    }
    private void AutoSpawnEnemy()
    {
        if (numberEnemySpawn < maxEnemySpawn)
        {
            numberEnemySpawn += stepNumberEnemy;
        }
        int targetRange = Random.Range(10, maxTargetRange);
        SpawnEnemy(numberEnemySpawn, targetRange);
    }
    private void SpawnEnemy(int number,int targetRange)
    {
        enemySpawner.SpawnEnemy(number, targetRange);
    }
}
