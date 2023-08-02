public struct Record
{
}

#region Camera Shaking
[System.Serializable]
public struct CameraShakingInfo
{
    public float shake_duration;
    public float shake_amount;
    public float decrease_factor;
}
#endregion

#region User Data
[System.Serializable]
public struct RecordUserData
{
    public bool is_first_open;
    public Info info;
    public Currency currency;
    public RecordItem[] inventory_characters;
    public RecordItem[] inventory_weapons;
    public RecordItem[] inventory_consumables;
    public RecordUserProgress current_progress;

    [System.Serializable]
    public struct Info
    {
    }

    [System.Serializable]
    public struct Currency
    {
        public int gold;
    }
}
#endregion

#region Shop
[System.Serializable]
public struct RecordShopItem
{
    public int id;
    public int price_gold;
    public float price_money;
    public string product_id;
    public string product_name;
    public string iap_type; 
}
#endregion

#region Reward
[System.Serializable]
public struct RecordReward
{
    public int id;
    public string name;
    public string icon;
    public string description;
    public int amount;
}
#endregion

#region Effect
[System.Serializable]
public struct RecordEffect
{
    public string type;
    public string description;
}
#endregion

#region Item
[System.Serializable]
public struct RecordItem
{
    public string id;
    public string name;
    public string description;
    public string icon;
    public string type;
    public EffectValue[] effect_values;
    public bool is_equiped;

    [System.Serializable]
    public struct EffectValue
    {
        public string id;
        public float value;
    }
}
#endregion

#region Item Equip
[System.Serializable]
public struct RecordItemEquip
{
    public string id;
    public bool is_equiped;
}
#endregion

#region Main Menu
[System.Serializable]
public struct RecordLevelMainMenu
{
    public int chapter;
    public int level;
    public string title;
    public string icon;
}
#endregion

#region User Progress
[System.Serializable]
public struct RecordUserProgress
{
    public int chapter;
    public int level;
}
#endregion
