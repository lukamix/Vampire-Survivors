using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Animator animator;
    public void SetTimeText(int time_count)
    {
        int mm = (int)time_count / 60;
        int ss = time_count - mm * 60;
        timeText.text = (mm>=10?$"{mm}":"0"+$"{mm}")+":"+(ss>=10?$"{ss}":"0"+$"{ss}");
    }
    public void SetBossTime()
    {
        animator.Play("BossTime");
    }
    public void SetEnemyTime()
    {
        animator.Play("EnemyTime");
    }
}
