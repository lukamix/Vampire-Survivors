using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneBackground : MonoBehaviour
{
    public GameObject DecorGameObject;
    public GameObject Logo;
    public GameObject ActiveAfterObject;
    private void Start()
    {
        CheckActive(DecorGameObject);
        CheckActive(Logo);
        CheckActive(ActiveAfterObject);
    }
    private void CheckActive(GameObject go)
    {
        if (go != null && go.activeSelf)
        {
            go.SetActive(false);
        }
    }
    public void OnBackGroundAnimationComplete()
    {
        DecorGameObject.SetActive(true);
        Logo.SetActive(true);
        StartCoroutine(SetActiveLastObject());
    }
    IEnumerator SetActiveLastObject()
    {
        yield return new WaitForSeconds(1);
        ActiveAfterObject.SetActive(true);
    }
}
