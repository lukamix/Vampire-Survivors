using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : DucSingleton<ItemManager>
{
    public ItemPoolManager itemPool;
    public ItemPoolManager coinPool;
    public ItemPoolManager chestPool;
    public ItemPoolManager foodPool;
}
