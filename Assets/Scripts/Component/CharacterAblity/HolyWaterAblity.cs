using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterAblity : Ablities
{
    [SerializeField] private ParticleSystem holyWaterPrefab;
    [SerializeField] private HolyWaterGlass glassOfHolyWaterPrefab;
    private List<HolyWaterGlass> listOfGlass;
    private List<ParticleSystem> listOfHolyWater;

    private float distanceToPlayer = 5; //Random Value
    private bool isAblitying = true;
    private float stepTime = 10;
    private float offSetTime = 0.2f;
    private int numberGlassDrop = 2;
    private Coroutine corDropHolyWater;
    private void Awake()
    {
        listOfGlass = new List<HolyWaterGlass>();
        listOfHolyWater = new List<ParticleSystem>();
        Observer.Instance.AddObserver(ObserverKey.HolyWaterGlassBreak, DoWhenHolyWaterGlassBreak);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.HolyWaterGlassBreak, DoWhenHolyWaterGlassBreak);
    }
    private void DoWhenHolyWaterGlassBreak(object data)
    {
        HolyWaterGlass glass = (HolyWaterGlass)data;
        ParticleSystem holyWater = GetAvailablHolyWater();
        HolyWater holyWaterScript = holyWater.GetComponent<HolyWater>();
        holyWaterScript.SetDame(level);
        holyWater.transform.position = glass.transform.position;
        glass.gameObject.SetActive(false);
        holyWater.gameObject.SetActive(true);
        holyWater.Play();
    }
    private HolyWaterGlass GetAvailableGlass()
    {
        foreach(HolyWaterGlass glass in listOfGlass)
        {
            if (!glass.gameObject.activeSelf)
            {
                return glass;
            }
        }
        HolyWaterGlass glassIns = Instantiate(glassOfHolyWaterPrefab, null);
        listOfGlass.Add(glassIns);
        glassIns.gameObject.SetActive(false);
        return glassIns;
    }
    private ParticleSystem GetAvailablHolyWater()
    {
        foreach (ParticleSystem glass in listOfHolyWater)
        {
            if (!glass.gameObject.activeSelf)
            {
                return glass;
            }
        }
        ParticleSystem holyWater = Instantiate(holyWaterPrefab, null);
        listOfHolyWater.Add(holyWater);
        holyWater.gameObject.SetActive(false);
        return holyWater;
    }
    private void FixedUpdate()
    {
        if (!isAblitying)
        {
            corDropHolyWater = StartCoroutine(IEDropHolyWater());
        }
    }
    public override void DoAblities()
    {
        if (isStartDoing)
        {
            numberGlassDrop = level;
            isStartDoing = false;
        }
        else
        {
            level++;
            numberGlassDrop++;
        }
        if (level <= 0) return;
        if (corDropHolyWater != null)
        {
            StopCoroutine(corDropHolyWater);
        }
        isAblitying = false;
    }
    private IEnumerator IEDropHolyWater()
    {
        isAblitying = true;
        for(int i=0;i< numberGlassDrop; i++)
        {
            HolyWaterGlass glass = GetAvailableGlass();
            glass.transform.position = transform.position;
            glass.gameObject.SetActive(true);
            float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector2 targetPosition = (Vector2)transform.position + distanceToPlayer * new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            glass.CalculatePosition(transform.position, targetPosition);
            yield return new WaitForSeconds(offSetTime);
        }
        yield return new WaitForSeconds(stepTime);
        isAblitying = false;
    }
}
