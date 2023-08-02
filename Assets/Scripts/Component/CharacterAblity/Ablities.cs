using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ablities : MonoBehaviour
{
    public int id;
    public int level = 0;
    protected bool isPassiveAblity =false;
    protected bool isStartDoing = true;
    protected virtual void Start()
    {
        level = AblityData.listAb()[id].ablityLevel;
        DoAblities();
    }
    public virtual void DoAblities()
    {

    }
}
