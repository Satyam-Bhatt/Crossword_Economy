using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WordBundle : MonoBehaviour
{
    public float length = 5f;
    public Vector2 direction = Vector2.right;
    public GameManager gameManager;

    [Space(10)]
    [Header("Second Word")]
    public bool secondWord = false;
    public Vector2 direction2 = Vector2.zero;
    public float length2 = 0f;

    public bool wordComplete { get; private set; } = false;
    public bool wordComplete2 { get; private set; } = false;

    public List<InputFieldData> inputFieldDatas1 = new List<InputFieldData>();
    public List<InputFieldData> inputFieldDatas2 = new List<InputFieldData>();

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction.normalized, length);
        foreach (RaycastHit2D hitInfo in hit)
        {
            if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
            {
                if(hitInfo.transform.GetComponent<InputFieldData>() != null)
                {
                    inputFieldDatas1.Add(hitInfo.transform.GetComponent<InputFieldData>());
                }
            }
        }

        if(secondWord)
        {
            hit = Physics2D.RaycastAll(transform.position, direction2.normalized, length2);
            foreach (RaycastHit2D hitInfo in hit)
            {
                if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
                {
                    if (hitInfo.transform.GetComponent<InputFieldData>() != null)
                    {
                        inputFieldDatas2.Add(hitInfo.transform.GetComponent<InputFieldData>());
                    }
                }
            }
        }
    }

    //private void Update()
    //{
    //}

    public bool WordCompletionCheck_1(bool isDragging)
    {
        bool completeWord = false;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction.normalized, length);
        bool found = true;
        string word = "";
        if (hit != null)
        {
            foreach (RaycastHit2D hitInfo in hit)
            {
                if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
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
        if (found)
        {
            if (ReadFromJSON.Instance.commonWordsList.ContainsKey(word.ToUpper()))
            {
                Debug.Log("Word is valid: " + word);

                //if(isDragging) gameManager.UpdateScore(1);

                foreach (RaycastHit2D hitInfo in hit)
                {
                    if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
                    {
                        hitInfo.transform.GetComponent<TMP_InputField>().enabled = false;
                    }

                    if (hitInfo.transform.GetComponent<UnityEngine.UI.Image>() != null)
                    {
                        Color originalColor = hitInfo.transform.GetComponent<UnityEngine.UI.Image>().color;
                        hitInfo.transform.GetComponent<UnityEngine.UI.Image>().color = originalColor * Color.green;
                    }
                }

                ReadFromJSON.Instance.commonWordsList.Remove(word.ToUpper());

                foreach (RaycastHit2D hitInfo in hit)
                {
                    if(hitInfo.transform.GetComponent<Tutorial>() != null)
                        hitInfo.transform.GetComponent<Tutorial>().enabled = false;
                }

                if (ReadFromJSON.Instance.sceneName != "Level-1" && ReadFromJSON.Instance.sceneName != "Level-2")
                {
                    StartCoroutine(CalculateScore_1());
                }
                completeWord = true;
            }
            else
            {
                completeWord = false;
                //Debug.Log("Word is not valid: " + word);
            }
        }

        return completeWord;

    }

    public bool WordCompletionCheck_2(bool isDragging)
    {
        bool completeWord = false;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction2.normalized, length2);
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

                //if (isDragging) gameManager.UpdateScore(1);

                foreach (RaycastHit2D hitInfo in hit)
                {
                    if (hitInfo.transform.GetComponent<TMP_InputField>() != null)
                    {
                        hitInfo.transform.GetComponent<TMP_InputField>().enabled = false;
                    }

                    if (hitInfo.transform.GetComponent<UnityEngine.UI.Image>() != null)
                    {
                        Color originalColor = hitInfo.transform.GetComponent<UnityEngine.UI.Image>().color;
                        hitInfo.transform.GetComponent<UnityEngine.UI.Image>().color = originalColor * Color.green;
                    }
                }
                if (ReadFromJSON.Instance.sceneName != "Level-1" && ReadFromJSON.Instance.sceneName != "Level-2")
                {
                    StartCoroutine(CalculateScore_2());
                }
                completeWord = true;
            }
            else
            {
                completeWord = false;
                //Debug.Log("Word 2 is not valid: " + word2);
            }
        }

        return completeWord;
    }

    IEnumerator CalculateScore_1()
    {
        yield return new WaitForEndOfFrame();
        int score = 0;
        foreach (InputFieldData inputFieldData in inputFieldDatas1)
        {
            if (inputFieldData.dragged) score = score + 10;
            else score = score + 5;
        }
        score = score + 10;
        gameManager.UpdatePoints(score);
    }

    IEnumerator CalculateScore_2()
    {
        yield return new WaitForEndOfFrame();
        int score = 0;
        foreach (InputFieldData inputFieldData in inputFieldDatas2)
        {
            if (inputFieldData.dragged) score = score + 10;
            else score = score + 5;
        }
        score = score + 10;
        gameManager.UpdatePoints(score);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction.normalized * length);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction2.normalized * length2);
    }
}
