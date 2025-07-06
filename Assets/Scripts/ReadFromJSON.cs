using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int totalMoves = 0;
    
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChange;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChange;
    }

    private void Start()
    {
        wordData = JsonUtility.FromJson<WordData>(allWords.text);

        foreach (string word in wordData.commonWords)
        {
            if (word.Length > 1)
            {
                string upperWord = word.ToUpper();
                if (!commonWordsList.ContainsKey(upperWord))
                {
                    commonWordsList.Add(upperWord, true);
                }
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

    public void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            totalMoves = 1;
            FindObjectOfType<GameManager>().resourcesAmount = totalMoves;
        }

        WordReload();
    }

    public void UpdateTotalMoves(int num)
    {
        totalMoves = num;
    }
}
