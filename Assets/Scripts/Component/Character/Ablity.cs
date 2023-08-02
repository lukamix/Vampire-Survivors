using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ablity : MonoBehaviour
{
    [SerializeField] private List<Ablities> listCharacterAblities = new List<Ablities>();
    private void Start()
    {
        Observer.Instance.AddObserver(ObserverKey.EquipUpgrade, DoAblity);
    }
    public int GetAblityLevel(int ablityID)
    {
        foreach (Ablities ab in listCharacterAblities)
        {
            if (ab.id == ablityID)
            {
                return ab.level;
            }
        }
        return 0;
    }
    private void DoAblity(object data)
    {
        int ablityID = (int)data;
        foreach(Ablities ab in listCharacterAblities)
        {
            if(ab.id == ablityID)
            {
                ab.DoAblities();
                CanvasManager.Instance.ablityCanvas.DoCanvasAblity(ablityID);
                break;
            }
        }
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(ObserverKey.EquipUpgrade, DoAblity);
    }
}
