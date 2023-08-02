using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;
    private string loadingString;
    private int number = 0;
    // Start is called before the first frame update
    void Start()
    {
        loadingString = loadingText.text;
        StartCoroutine(RunText());
    }
    private IEnumerator RunText()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            number++;
            string tempString = loadingString;
            if (number > 3)
            {
                number = 0;
                loadingText.text = tempString;
            }
            else
            {
                for(int i = 0; i < number; i++)
                {
                    tempString += ".";
                }
                loadingText.text = tempString;
            }
        }
    }
}
