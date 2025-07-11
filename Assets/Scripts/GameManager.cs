using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private char letter_Hold;

    [SerializeField] private GameObject letterShow;
    [SerializeField] private TMP_Text resources;
    public int resourcesAmount = 0;
    public int points = 0;
    [SerializeField] private int winAmount = 0;
    [SerializeField] private GameObject LooseScreen;
    public WordBundle[] wordBundles;
    public GameObject WinScreen;

    private GameObject letterShow_Container;
    private GameObject text_Store = null;
    private Canvas canvas;
    public TMP_Text pointText;

    // Touch-specific variables
    public bool isDragging = false;
    private int activeTouchId = -1;
    private bool fingerDragging = false;

    public WinChecker winChecker;

    private void Awake()
    {
        winChecker = FindObjectOfType<WinChecker>();
        wordBundles = FindObjectsOfType<WordBundle>();
    }

    private void Start()
    {
        resourcesAmount = ReadFromJSON.Instance.totalMoves;

        if (ReadFromJSON.Instance.storedPoints == 0)
        {
            ReadFromJSON.Instance.points = 0;
        }

        points = ReadFromJSON.Instance.points;

        LooseScreen.SetActive(false);
        WinScreen.SetActive(false);
        canvas = GameObject.Find("WorldCanvas").GetComponent<Canvas>();
        resources.text = resourcesAmount.ToString();

        foreach (WordBundle w in wordBundles)
        {
            w.WordCompletionCheck_1(isDragging);
            w.WordCompletionCheck_2(isDragging);
        }

        if(ReadFromJSON.Instance.sceneName != "Level-1" && ReadFromJSON.Instance.sceneName != "Level-2")
        {
            pointText = GameObject.Find("Points").transform.GetChild(1).GetComponent<TMP_Text>();
            pointText.text = points.ToString();
        }
    }

    void Update()
    {
        // Handle keyboard input (unchanged)
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Space)) // Not working for on screen
        {
            if (char.IsLetter(Input.inputString[0]) && resourcesAmount > 0)
            {
                ResourceCalculateAndEffect();
            }
        }

        //if (resourcesAmount < 0)
        //{
        //    LooseScreen.SetActive(true);
        //}

        // Handle touch input
        HandleTouchInput();

        // Also handle mouse input for testing in editor
        HandleMouseInput();
    }

    private void HandleTouchInput()
    {
        // Handle touch began
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled && touch.fingerId == activeTouchId)
                {
                    RestoreKeyboard();
                    //Debug.Log("ENDED  " + fingerDragging + " " + isDragging);
                    //if (!fingerDragging && isDragging)
                    //{
                    //    ShowKeyboard();
                    //}
                    //else if (fingerDragging && isDragging)
                    //{
                    //    HideKeyboard();
                    //}
                    //fingerDragging = false;
                }

                if (touch.phase == TouchPhase.Began && !isDragging)
                {
                    OnTouchBegan(touch.position, touch.fingerId);
                    break;
                }
                else if (touch.phase == TouchPhase.Moved && isDragging && touch.fingerId == activeTouchId)
                {
                    fingerDragging = true;
                    HideKeyboard();
                    OnTouchMoved(touch.position);
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && touch.fingerId == activeTouchId)
                {
                    OnTouchEnded(touch.position);
                    break;
                }

  
            }
        }
    }

    private void HandleMouseInput()
    {
        // Keep mouse input for editor testing
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            OnTouchBegan(Input.mousePosition, -1);
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            OnTouchMoved(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            OnTouchEnded(Input.mousePosition);
        }
    }

    private void OnTouchBegan(Vector2 screenPosition, int touchId)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Ray ray = new Ray(worldPosition, Vector3.forward);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);

        if (hitInfo)
        {
            if(hitInfo.transform.CompareTag("DefaultLetter")) return;

            TMP_InputField text = hitInfo.transform.gameObject.GetComponentInChildren<TMP_InputField>();
            text_Store = hitInfo.transform.gameObject;
            if (text != null)
            {
                if (text.text.Length == 1)
                {
                    if (char.IsLetter(text.text[0]))
                    {
                        letter_Hold = hitInfo.transform.gameObject.GetComponentInChildren<TMP_Text>().text[0];
                        letterShow_Container = Instantiate(letterShow, new Vector2(worldPosition.x, worldPosition.y), Quaternion.identity);
                        letterShow_Container.transform.SetParent(canvas.transform, false);
                        letterShow_Container.GetComponent<TMP_Text>().text = letter_Hold.ToString().ToUpper();

                        isDragging = true;
                        activeTouchId = touchId;
                    }
                }
            }
        }
    }

    private void OnTouchMoved(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (letterShow_Container != null)
        {
            // For UI elements, we need to convert screen position to canvas position
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPosition,
                canvas.worldCamera,
                out canvasPosition);

            letterShow_Container.transform.localPosition = canvasPosition;
        }
    }

    private void OnTouchEnded(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Ray ray = new Ray(worldPosition, Vector3.forward);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);

        if (hitInfo)
        {
            TMP_InputField text = hitInfo.transform.gameObject.GetComponentInChildren<TMP_InputField>();
            if (text != null)
            {
                if (letter_Hold != '\0' && char.IsLetter(letter_Hold) && text_Store != hitInfo.transform.gameObject)
                {
                    if (hitInfo.transform.GetComponent<Image>().color != Color.green)
                        text.text = letter_Hold.ToString();
                }
            }
        }

        if (letterShow_Container != null) Destroy(letterShow_Container);

        if (letter_Hold != '\0') letter_Hold = '\0';
        if (text_Store != null) text_Store = null;

        if(isDragging)
        {
            //StartCoroutine(RestoreKeyboardAfterDelay());
        }

        isDragging = false;
        activeTouchId = -1;
    }

    public void ShowKeyboard()
    {
#if UNITY_ANDROID || UNITY_IOS
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
#endif
    }

    private UnityEngine.UI.InputField lastActiveInputField;
    private TMPro.TMP_InputField lastActiveTMPInputField;

    public void HideKeyboard()
    {
        // Store reference to currently active field before deactivating
        var inputFields = FindObjectsOfType<UnityEngine.UI.InputField>();
        foreach (var field in inputFields)
        {
            if (field.isFocused)
            {
                lastActiveInputField = field;
                field.DeactivateInputField();
            }
        }

        var tmpInputFields = FindObjectsOfType<TMPro.TMP_InputField>();
        foreach (var field in tmpInputFields)
        {
            if (field.isFocused)
            {
                lastActiveTMPInputField = field;
                field.DeactivateInputField();
            }
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void RestoreKeyboard()
    {
        if (lastActiveInputField != null)
        {
            lastActiveInputField.ActivateInputField();
            lastActiveInputField = null;
        }

        if (lastActiveTMPInputField != null)
        {
            lastActiveTMPInputField.ActivateInputField();
            lastActiveTMPInputField = null;
        }
    }

    private IEnumerator RestoreKeyboardAfterDelay()
    {
        float newTime = Time.time + 5f;
        while(Time.time < newTime)
        {
            HideKeyboard();
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        RestoreKeyboard();
    }

    public void LevelWin()
    {
        WinScreen.SetActive(true);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings == sceneIndex)
        {
            WinScreen.transform.GetChild(1).gameObject.SetActive(false);
            WinScreen.transform.GetChild(0).GetComponent<TMP_Text>().text = "GAME COMPLETE";
            UpdatePoints(100 * resourcesAmount);
        }

        if (ReadFromJSON.Instance.sceneName != "Level-1" && ReadFromJSON.Instance.sceneName != "Level-2" && ReadFromJSON.Instance.sceneName != "Level-9")
        {
            UpdatePoints(50 * resourcesAmount);
        }

        ReadFromJSON.Instance.UpdateTotalMoves(resourcesAmount + winAmount);
        resourcesAmount = ReadFromJSON.Instance.totalMoves;
        resources.text = resourcesAmount.ToString();
    }

    public void Restart()
    {
        ReadFromJSON.Instance.points = ReadFromJSON.Instance.storedPoints;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        ReadFromJSON.Instance.storedPoints = points;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Replay()
    {
        ReadFromJSON.Instance.points = 0;
        ReadFromJSON.Instance.storedPoints = 0;
        ReadFromJSON.Instance.totalMoves = 1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void UpdateScore(int num)
    {
        resourcesAmount = resourcesAmount + num;
        resources.text = resourcesAmount.ToString();
    }

    public void ValueChange()
    {
        foreach(WordBundle w in wordBundles)
        {
            w.WordCompletionCheck_1(isDragging);
            w.WordCompletionCheck_2(isDragging);
        }

        if(!isDragging && resourcesAmount > 0)
        {
            ResourceCalculateAndEffect();
        }
    }

    public void UpdatePoints(int num)
    {
        points = points + num;
        pointText.text = points.ToString();
        ReadFromJSON.Instance.points = points;
    }

    private void ResourceCalculateAndEffect()
    {
        resourcesAmount--;
        resources.text = resourcesAmount.ToString();

        if (resourcesAmount == 0) winChecker.DisableAllInputFields();
    }
}