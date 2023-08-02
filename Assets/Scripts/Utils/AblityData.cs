
using System.Collections.Generic;
using UnityEngine;

public static class AblityData
{
    private static List<AbData> _listAb;
    public static List<AbData> listAb()
    {
        int characterID = PlayerPrefs.GetInt(PlayerPrefsString.characterString, 0);

        int aLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "0", characterID == 0 ? 1 : 0);
        int bLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "1", 0);
        int cLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "2", 0);
        int dLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "3", characterID == 1 ? 1 : 0);
        int eLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "4", 1);
        int fLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "5", 1);
        int gLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "6", 1);
        int hLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "7", 0);
        int iLevel = PlayerPrefs.GetInt(PlayerPrefsString.ablityString + "8", characterID == 1 ? 1 : 0);
        if (_listAb == null)
        {
            _listAb = new List<AbData>();
            Sprite[] all = Resources.LoadAll<Sprite>("Textures/items");
            Sprite aS = all[14];
            AbData a = new AbData(0, aLevel,"Knives Out", "Phóng dao liên tục vào kẻ địch.", aS);
            Sprite bS = all[32];
            AbData b = new AbData(1, bLevel, "King Bible", "Những cuốn sách ma thuật này liệu có phải là ma thuật bóng tối ?", bS);
            Sprite cS = all[146];
            AbData c = new AbData(2, cLevel, "Santa Water", "Nước thánh có thể tiêu diệt mọi kẻ thù !!!",cS);
            Sprite dS = all[147];
            AbData d = new AbData(3, dLevel, "Whip", "Quất roi liên tục vào kẻ địch có ý định tiếp cận.", dS);
            Sprite eS = all[76];
            AbData e = new AbData(4, eLevel, "Wings", "Mỗi cấp độ tăng tốc độ chạy của nhân vật lên 10%.", eS);
            Sprite fS = all[58];
            AbData f = new AbData(5, fLevel, "Attractorb", "Tăng tầm nhặt vật phẩm của nhân vật.", fS);
            Sprite gS = all[169];
            AbData g = new AbData(6, gLevel, "Spinach", "Tăng sát thương của nhân vật.", gS);
            Sprite hS = all[18];
            AbData h = new AbData(7, hLevel, "Eight The Sparrow", "Những viên đạn bắn ra từ khẩu súng này có thể nảy.", hS);
            Sprite iS = all[39];
            AbData i = new AbData(8, iLevel, "Dart", "Boomerang có thể đảo chiều.", iS);
            _listAb.Add(a);
            _listAb.Add(b);
            _listAb.Add(c);
            _listAb.Add(d);
            _listAb.Add(e);
            _listAb.Add(f);
            _listAb.Add(g);
            _listAb.Add(h);
            _listAb.Add(i);
        }
        else
        {
            _listAb[0].ablityLevel = aLevel;
            _listAb[1].ablityLevel = bLevel;
            _listAb[2].ablityLevel = cLevel;
            _listAb[3].ablityLevel = dLevel;
            _listAb[4].ablityLevel = eLevel;
            _listAb[5].ablityLevel = fLevel;
            _listAb[6].ablityLevel = gLevel;
            _listAb[7].ablityLevel = hLevel;
            _listAb[8].ablityLevel = iLevel;
        }
        return _listAb;
    }
}
public class AbData
{
    public AbData(int id,int level,string name,string content,Sprite sprite)
    {
        ablityId = id;
        ablityLevel = level;
        ablityName = name;
        ablityContent = content;
        ablitySprite = sprite;
    }
    public int ablityId;
    public int ablityLevel;
    public string ablityName;
    public string ablityContent;
    public Sprite ablitySprite;

    public void UpgradeAb()
    {
        ablityLevel++;
        PlayerPrefs.SetInt(PlayerPrefsString.ablityString + ablityId, ablityLevel);
    }
}