using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AblityCanvas : MonoBehaviour
{
    [SerializeField] private AblityCanvasItem ablityCanvasItemPrefab;
    [SerializeField] private List<AblityCanvasItem> listCharacterAblities = new List<AblityCanvasItem>();
    public void DoCanvasAblity(int ablity_id)
    {
        foreach(AblityCanvasItem abci in listCharacterAblities)
        {
            if (abci.GetAblityId() == ablity_id)
            {
                abci.IncreaseAblityLevel();
                return;
            }
        }
        AddAblityItem(ablity_id);
    }
    private void AddAblityItem(int ablity_id)
    {
        AblityCanvasItem newAblity = Instantiate(ablityCanvasItemPrefab, transform);
        newAblity.InitAblity(ablity_id);
        listCharacterAblities.Add(newAblity);
    }
}
