using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class WordBundle : MonoBehaviour
{
    public float length = 5f;
    public Vector2 direction = Vector2.right;

    [Space(10)]
    [Header("Second Word")]
    public bool secondWord = false;
    public Vector2 direction2 = Vector2.zero;
    public float length2 = 0f;

    public bool wordComplete { get; private set; } = false;

    private void Update()
    {
        if (wordComplete) return;

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction.normalized, length);
        bool found = true;
        string word = "";
        if (hit != null)
        {
            foreach (RaycastHit2D hitInfo in hit)
            {
                if(hitInfo.transform.GetComponent<TMP_InputField>() != null)
                {
                    if (hitInfo.transform.GetComponent<TMP_InputField>().text == "")
                    {
                        found = false;
                        break;
                    }
                    word += hitInfo.transform.GetComponent<TMP_InputField>().text[0];
                }
            }
        }
        if(found)
        {

            if (ReadFromJSON.Instance.commonWordsList.ContainsKey(word.ToUpper()))
            {
                Debug.Log("Word is valid: " + word);
                foreach (RaycastHit2D hitInfo in hit)
                {
                    if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
                    {
                        hitInfo.transform.GetComponent<TMP_InputField>().enabled = false;
                    }

                    if(hitInfo.transform.GetComponent<UnityEngine.UI.Image>() != null)
                    {
                        hitInfo.transform.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    }
                }
                if(!secondWord) wordComplete = true;
            }
            else
            {
                Debug.Log("Word is not valid: " + word);
            }
        }

        if (secondWord)
        {
            hit = Physics2D.RaycastAll(transform.position, direction2.normalized, length2);
            bool found2 = true;
            string word2 = "";
            if (hit != null)
            {
                foreach (RaycastHit2D hitInfo in hit)
                {
                    if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
                    {
                        if (hitInfo.transform.GetComponent<TMP_InputField>().text == "")
                        {
                            found2 = false;
                            break;
                        }
                        word2 += hitInfo.transform.GetComponent<TMP_InputField>().text[0];
                    }
                }
            }

            if (found2)
            {
                if (ReadFromJSON.Instance.commonWordsList.ContainsKey(word2.ToUpper()))
                {
                    Debug.Log("Word 2 is valid: " + word2);
                    foreach (RaycastHit2D hitInfo in hit)
                    {
                        if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
                        {
                            hitInfo.transform.GetComponent<TMP_InputField>().enabled = false;
                        }

                        if (hitInfo.transform.GetComponent<UnityEngine.UI.Image>() != null)
                        {
                            hitInfo.transform.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                        }
                    }
                    secondWord = false;
                }
                else
                {
                    Debug.Log("Word 2 is not valid: " + word2);
                }
            }
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction.normalized * length);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction2.normalized * length2);
    }
}
