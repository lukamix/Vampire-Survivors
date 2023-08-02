using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : DucSingleton<CanvasManager>
{
    private void Start()
    {
        expBarController.SetEXP(0);
        for(int i = 0; i < AblityData.listAb().Count; i++)
        {
            if (AblityData.listAb()[i].ablityLevel >= 1)
            {
                ablityCanvas.DoCanvasAblity(i);
            }
        }
    }
    public ExpBarController expBarController;
    public AblityCanvas ablityCanvas;
    public TimeCounter timeCounter;
    public EnemyCounter enemyCounter;
    public GoldCounter goldCounter;
}
