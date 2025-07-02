using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadFromJSON : MonoBehaviour
{
    private static ReadFromJSON _instance;

    public static ReadFromJSON Instance
    {
        get
        {
            if(_instance != null) return _instance;

            _instance = FindObjectOfType<ReadFromJSON>();

            if(_instance == null)
            {
                GameObject go = new GameObject("ReadFromJSON");
                _instance = go.AddComponent<ReadFromJSON>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    [SerializeField] private TextAsset allWords;
    
    public Dictionary<string, bool> commonWordsList = new Dictionary<string, bool>();

    [System.Serializable]
    public class WordData
    {
        public string description;
        public string[] commonWords;
    }

    [SerializeField] private WordData wordData = new WordData();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        wordData = JsonUtility.FromJson<WordData>(allWords.text);

        foreach (string word in wordData.commonWords)
        {
            if(word.Length > 1)
            {
                commonWordsList.Add(word.ToUpper(), true);
            }
        }
    }

    public void WordReload()
    {
        wordData = JsonUtility.FromJson<WordData>(allWords.text);

        commonWordsList.Clear();

        foreach (string word in wordData.commonWords)
        {
            if (word.Length > 1)
            {
                commonWordsList.Add(word.ToUpper(), true);
            }
        }
    }
}
