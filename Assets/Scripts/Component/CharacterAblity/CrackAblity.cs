using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackAblity : Ablities
{
    private float offSetTime = 2;
    private float currentOffSetTime;
    [SerializeField] private Cracks crackPrefab;
    private Animator animator;
    private bool isCracking = true;
    private Coroutine corCracking;
    private void Awake()
    {
        isPassiveAblity = PlayerPrefs.GetInt(PlayerPrefsString.characterString, 0) == 1;
    }
    private void FixedUpdate()
    {
        if (!isCracking)
        {
            currentOffSetTime = offSetTime/level;
            corCracking = StartCoroutine(IECracking());
        }
    }
    public override void DoAblities()
    {
        if (isStartDoing)
        {
            if (level <= 0 && isPassiveAblity) level = 1;
            isStartDoing = false;
        }
        else
        {
            level++;
        }
        if (level <= 0) return;
        if (corCracking != null)
        {
            StopCoroutine(corCracking);
        }
        isCracking = false;
    }
    private IEnumerator IECracking()
    {
        isCracking = true;
        if (animator == null)
        {
            Cracks crack = Instantiate(crackPrefab, transform.position + Vector3.up,Quaternion.identity, null);
            foreach(Crack cr in crack.crackList)
            {
                cr.SetDame(level);
            }
            animator = crack.GetComponent<Animator>();
        }
        else
        {
            animator.transform.position = transform.position + Vector3.up;
        }
        animator.Play("DoCrack");
        yield return new WaitForSeconds(offSetTime);
        animator.Play("None");
        isCracking = false;
    }
}
