using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageMoveAndFade : MonoBehaviour
{
    [Header("Animation Settings")]
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveDuration = 2f;
    public float fadeDuration = 1f;
    public Ease moveEase = Ease.OutQuad;
    public Ease fadeEase = Ease.InQuad;

    [Header("References")]
    public Image targetImage;

    private RectTransform imageRect;
    private Sequence animationSequence;

    public bool swapPosition = false;

    //public bool positionSet = false;

    void Start()
    {
        // Get the Image component if not assigned
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (targetImage == null)
        {
            Debug.LogError("No Image component found! Please assign an Image to the targetImage field.");
            return;
        }

        if(swapPosition)
        {
            Vector3 temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;
        }

        imageRect = targetImage.GetComponent<RectTransform>();

        // Set initial position and alpha
        imageRect.anchoredPosition = startPosition;
        targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 1f);

        // Start the animation loop
        StartAnimationLoop();
    }

    //private void Update()
    //{
    //    if(positionSet)
    //    {
    //        imageRect.anchoredPosition = startPosition;
    //    }
    //    else
    //    {
    //        imageRect.anchoredPosition = endPosition;
    //    }
    //}

    void StartAnimationLoop()
    {
        // Kill any existing sequence
        if (animationSequence != null)
            animationSequence.Kill();

        // Create the sequence
        animationSequence = DOTween.Sequence();

        // Move from start to end position
        animationSequence.Append(imageRect.DOAnchorPos(endPosition, moveDuration).SetEase(moveEase));

        // Fade out
        animationSequence.Append(targetImage.DOFade(0f, fadeDuration).SetEase(fadeEase));

        // Reset position back to start (happens instantly while faded out)
        animationSequence.AppendCallback(() => {
            imageRect.anchoredPosition = startPosition;
        });

        // Fade back in
        animationSequence.Append(targetImage.DOFade(1f, fadeDuration).SetEase(fadeEase));

        // Set the sequence to loop infinitely
        animationSequence.SetLoops(-1, LoopType.Restart);

        // Play the sequence
        animationSequence.Play();
    }

    void OnDestroy()
    {
        // Clean up the sequence when the object is destroyed
        if (animationSequence != null)
            animationSequence.Kill();
    }

    // Optional: Methods to control the animation
    public void PauseAnimation()
    {
        if (animationSequence != null)
            animationSequence.Pause();
    }

    public void ResumeAnimation()
    {
        if (animationSequence != null)
            animationSequence.Play();
    }

    public void StopAnimation()
    {
        if (animationSequence != null)
        {
            animationSequence.Kill();
            // Reset to initial state
            imageRect.anchoredPosition = startPosition;
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 1f);
        }
    }

    public void RestartAnimation()
    {
        StopAnimation();
        StartAnimationLoop();
    }

    // Helper method to set positions in the editor
    [ContextMenu("Set Start Position to Current")]
    void SetStartPositionToCurrent()
    {
        if (imageRect == null)
            imageRect = GetComponent<RectTransform>();

        startPosition = imageRect.anchoredPosition;
    }

    [ContextMenu("Set End Position to Current")]
    void SetEndPositionToCurrent()
    {
        if (imageRect == null)
            imageRect = GetComponent<RectTransform>();

        endPosition = imageRect.anchoredPosition;
    }
}