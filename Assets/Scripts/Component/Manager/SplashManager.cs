using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadGame());
    }
    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(1);

        ScenesManager.Instance.GetScene("Game", false);
    }
}
