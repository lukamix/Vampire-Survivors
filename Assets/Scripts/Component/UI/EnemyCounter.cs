using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyCount;
    private int numberEnemyDead = 0;
    public void IncreaseEnemyDead()
    {
        numberEnemyDead++;
        enemyCount.text = $"{numberEnemyDead}";
    }
}
