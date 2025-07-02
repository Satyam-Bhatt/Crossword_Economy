using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChecked : MonoBehaviour
{
    [SerializeField] private ReadFromJSON readFromJSON;

    public string wordToCheck = "hello";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(readFromJSON.commonWordsList.ContainsKey(wordToCheck.ToUpper()))
            {
                Debug.Log("Word is valid");
            }
            else
            {
                Debug.Log("Word is not valid");
            }
        }
    }
}
