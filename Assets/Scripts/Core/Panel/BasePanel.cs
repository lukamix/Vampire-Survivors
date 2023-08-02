using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [SerializeField]
    private Transform main;
    /*[SerializeField]
    private LeanTweenType tweenType = LeanTweenType.easeSpring;*/

    protected virtual void Awake()
    {
        /*if (main != null)
            main.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);*/
    }

    protected virtual void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }
    }

    protected virtual void OnEnable()
    {
        /*if(main != null)
        {
            main.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            main.LeanScale(Vector3.one, 0.4f).setEase(tweenType);
        }*/
    }

    public virtual void HidePanel()
    {
        PanelManager.Hide(this);
    }
}
