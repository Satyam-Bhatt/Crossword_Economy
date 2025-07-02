using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinChecker : MonoBehaviour
{
    public UnityEngine.UI.Image[] childImages;
    bool win = false;
    // Start is called before the first frame update
    void Start()
    {
        childImages = new UnityEngine.UI.Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childImages[i] = transform.GetChild(i).GetComponent<UnityEngine.UI.Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < childImages.Length; i++)
        {
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
            GameManager.Instance.WinScreen.SetActive(true);
        }
    }
}
