using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private string letter_Hold;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(Input.inputString);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = new Ray(mousePosition,Vector3.forward);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
            if(hitInfo)
            {
                TMP_InputField text = hitInfo.transform.gameObject.GetComponentInChildren<TMP_InputField>();
                if (text != null)
                {
                    if (text.text != "")
                    {
                        letter_Hold = hitInfo.transform.gameObject.GetComponentInChildren<TMP_Text>().text;
                    }
                }
                
            }
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = new Ray(mousePosition, Vector3.forward);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
            if (hitInfo)
            {
                TMP_InputField text = hitInfo.transform.gameObject.GetComponentInChildren<TMP_InputField>();
                if (text != null)
                {
                    if (letter_Hold != null)
                    {
                        text.text = letter_Hold;
                    }
                }

            }

            letter_Hold = null;
        }

    }
}
