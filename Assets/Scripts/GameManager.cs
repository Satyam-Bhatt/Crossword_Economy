using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string letter_Hold;

    [SerializeField] private GameObject letterShow;
    [SerializeField] private TMP_Text resources;
    [SerializeField] private int resourcesAmount = 0;
    [SerializeField] private GameObject LooseScreen;
    [SerializeField] private GameObject WinScreen;

    private GameObject letterShow_Container;
    private GameObject text_Store = null;
    private Canvas canvas;

    private void Start()
    {
        LooseScreen.SetActive(false);
        WinScreen.SetActive(false);
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        resources.text = resourcesAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Space))
        {
            if (char.IsLetter(Input.inputString[0]))
            {
                resourcesAmount--;
                resources.text = resourcesAmount.ToString();
            }
        }

        if(resourcesAmount < 0)
        {
            LooseScreen.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            WinScreen.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = new Ray(mousePosition,Vector3.forward);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
            if(hitInfo)
            {
                TMP_InputField text = hitInfo.transform.gameObject.GetComponentInChildren<TMP_InputField>();
                text_Store = hitInfo.transform.gameObject;
                if (text != null)
                {
                    if (text.text.Length == 1)
                    {
                        if (char.IsLetter(text.text[0]))
                        {
                            letter_Hold = hitInfo.transform.gameObject.GetComponentInChildren<TMP_Text>().text;
                            letterShow_Container = Instantiate(letterShow, new Vector2(mousePosition.x, mousePosition.y), Quaternion.identity);
                            letterShow_Container.transform.SetParent(canvas.transform, false);
                            letterShow_Container.GetComponent<TMP_Text>().text = letter_Hold;
                        }
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
                    if (letter_Hold != null && char.IsLetter(letter_Hold[0]) && text_Store != hitInfo.transform.gameObject)
                    {
                        text.text = letter_Hold;

                    }
                }
            }
            if (letterShow_Container != null)  Destroy(letterShow_Container);
            if (letter_Hold != null) letter_Hold = null;
            if(text_Store != null) text_Store = null;
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
