using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private DaiBang daibang;
    [HideInInspector] public Transform character;

    public void PlayerManagerStart()
    {
        int selectecdCharacter = PlayerPrefs.GetInt(PlayerPrefsString.characterString, 0);
        character = Instantiate(playerPrefabs[selectecdCharacter]).transform;
        Attribute att = character.GetComponent<Attribute>();
        att.SetDaiBang(daibang);
    }
}
