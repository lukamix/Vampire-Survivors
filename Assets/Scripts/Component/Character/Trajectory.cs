using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject dotParent;
    [SerializeField] private GameObject dotsPrefabs;
    private List<GameObject> dotsList;
    private int dotsNum = 10;
    private float maxScale = 0.15f;
    private float minScale = 0.05f;
    private int range = 4;

    private void Start()
    {
        InitBall();
    }
    private void InitBall()
    {
        dotsList = new List<GameObject>();
        for (int i = 0; i < dotsNum; i++)
        {
            GameObject dot = Instantiate(dotsPrefabs, dotParent.transform);
            dot.transform.localScale = (maxScale - i * (maxScale - minScale) / dotsNum) * Vector3.one;
            dotsList.Add(dot);
        }
        Hide();
    }
    float gravity = 9.8f;
    float GetInitialSpeed(float distance, float launchAngle)
    {
        float sine = Mathf.Sin(2 * launchAngle); //You can *not* cache this, since it is a parameter now.
        float squaredSpeed = distance * gravity / sine;
        return Mathf.Sqrt(squaredSpeed);
    }
    private float minAngle = 30f;
    private float maxAngle = 90f;
    public void Draw(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 offSet = endPoint - startPoint;
        Vector2 direction = offSet.normalized;
        float distance = offSet.magnitude > range ? range : offSet.magnitude;
        distance = distance <= 0 ? 0.01f : distance;
        float angle = (maxAngle - (maxAngle -minAngle)/(range) *distance) * Mathf.Deg2Rad;
        float speed = GetInitialSpeed(distance, angle);
        float timeSpacing = (distance / (speed * Mathf.Cos(angle)))/dotsNum;
        float anglebetween = Vector2.SignedAngle(direction, Vector2.right);
        for(int i = 1; i <= dotsNum; i++)
        {
            float x = speed * timeSpacing * i * Mathf.Cos(angle);
            float y = speed * timeSpacing * i * Mathf.Sin(angle) - 1 / 2f * gravity * timeSpacing * timeSpacing * i * i;
            Vector2 v = new Vector2(x, y);
            Vector2 rotatedVector = Quaternion.AngleAxis(-anglebetween, Vector3.forward) * v;
            
            dotsList[i-1].transform.position = rotatedVector + startPoint;
        }
    }
    public void Hide()
    {
        dotParent.SetActive(false);
    }
    public void Show()
    {
        dotParent.SetActive(true);
    }
}
