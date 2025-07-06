using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    GameManager gameManager;
    public GameObject TutorialPanel;

    public bool swap = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        TutorialPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputPosition = Vector3.zero;
        bool isPressed = false;
        bool isHeld = false;
        bool isReleased = false;

        // Check for mouse input (for editor/desktop)
        if (Input.mousePresent)
        {
            inputPosition = Input.mousePosition;
            isPressed = Input.GetMouseButtonDown(0);
            isHeld = Input.GetMouseButton(0);
            isReleased = Input.GetMouseButtonUp(0);
        }

        // Check for touch input (mobile devices)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get first touch
            inputPosition = touch.position;

            isPressed = touch.phase == TouchPhase.Began;
            isHeld = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
            isReleased = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

        if (isPressed)
        {
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(worldPosition, Vector3.forward);

            foreach (RaycastHit2D hit in hitInfo)
            {
                if (hit.transform.GetComponent<TMP_InputField>() != null)
                {
                    TMP_InputField inputField = hit.transform.GetComponent<TMP_InputField>();
                    if (inputField.text == "" && gameManager.resourcesAmount == 0)
                    {
                        TutorialPanel.SetActive(true);

                        if(swap)
                        {
                            FindObjectOfType<ImageMoveAndFade>().swapPosition = true;
                        }
                    }
                }
            }
        }

    }
}
