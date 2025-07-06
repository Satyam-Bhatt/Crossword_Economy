using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinChecker : MonoBehaviour
{
    public UnityEngine.UI.Image[] childImages;
    bool win = false;
    public GameManager gameManager;
    bool levelCompleted = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        childImages = new UnityEngine.UI.Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf) childImages[i] = transform.GetChild(i).GetComponent<UnityEngine.UI.Image>();

            TMP_InputField inputField = transform.GetChild(i).GetComponent<TMP_InputField>();
            if(inputField.text != "")
            {
                inputField.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(levelCompleted) return;
        
        for(int i = 0; i < childImages.Length; i++)
        {
            if(childImages[i] == null) continue;

            if (childImages[i].color == Color.green)
            {
                win = true;
            }
            else
            {
                win = false;
                break;
            }
        }
        if (win)
        {
            Debug.Log("You Win!");
            gameManager.LevelWin();

            if(FindObjectOfType<Tutorial>() != null) FindObjectOfType<Tutorial>().TutorialPanel.SetActive(false);

            levelCompleted = true;
        }
    }

    public void DisableAllInputFields()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //Debug.Log("Call");
            TMP_InputField inputField = transform.GetChild(i).GetComponent<TMP_InputField>();
            inputField.enabled = false;
        }
    }
}
