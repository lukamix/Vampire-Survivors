using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterData
{
    private static List<CharData> _listChar;
    public static List<CharData> listChar()
    {
        if (_listChar == null)
        {
            _listChar = new List<CharData>();
            Sprite[] allChars = Resources.LoadAll<Sprite>("Textures/characters");
            Sprite[] allItems = Resources.LoadAll<Sprite>("Textures/items");
            CharData char1 = new CharData(0, "Gennaro","Throw knives", allChars[175], allItems[14]);
            CharData char2 = new CharData(1, "Antonio", "Whip", allChars[237], allItems[147]);
            CharData char3 = new CharData(2, "Krochi", "Boomerang", allChars[232], allItems[39]);
            _listChar.Add(char1);
            _listChar.Add(char2);
            _listChar.Add(char3);
        }
        return _listChar;
    }
}
public class CharData
{
    public CharData(int id,string name,string des,Sprite charS,Sprite weaponS)
    {
        characterId = id;
        characterName = name;
        description = des;
        characterSprite = charS;
        weaponSprite = weaponS;
    }
    public int characterId;
    public string characterName;
    public string description;
    public Sprite characterSprite; 
    public Sprite weaponSprite;
}