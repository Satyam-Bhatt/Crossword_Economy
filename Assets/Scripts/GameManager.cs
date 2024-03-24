using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private string letter_Hold;

    [SerializeField] private GameObject letterShow;
    private GameObject letterShow_Container;
    private Canvas canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(Input.inputString);
        }

        if (Input.GetMouseButtonDown(0))
        {
            
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
                        letterShow_Container = Instantiate(letterShow, new Vector2(mousePosition.x, mousePosition.y), Quaternion.identity);
                        letterShow_Container.transform.SetParent(canvas.transform, false);
                        letterShow_Container.GetComponent<TMP_Text>().text = letter_Hold;
                    }
                }
                
            }
        }

        if(letterShow_Container != null)
        {
            letterShow_Container.transform.position = new Vector2(mousePosition.x, mousePosition.y);
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
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
            Destroy(letterShow_Container) ;
            letter_Hold = null;
        }

    }
}
