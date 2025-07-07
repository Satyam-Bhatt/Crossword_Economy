using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldData : MonoBehaviour
{
    public string inputText;
    public bool dragged;

    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnValueChange()
    {
        inputText = GetComponent<TMP_InputField>().text;
        if(gameManager.isDragging) dragged = true;
        else dragged = false;
    }
}
