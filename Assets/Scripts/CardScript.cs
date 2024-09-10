using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Canvas canvas;
    private bool isHovering = false;
    private bool isDragging = false;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    private float hoverScaleFactor = 1.5f;
    private float dragScaleFactor = 0.5f;
    private Vector3 hoverPosition;
    private float scaleSpeed = 5f;
    private float scaleDragSpeed = 10f;

    void Awake(){
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        hoverPosition = originalPosition + new Vector3(0, 100, 0);
    }

    void Update(){
        if (isHovering && !isDragging)
        {
            // Lerp the scale to make the image bigger when hovering
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originalScale * hoverScaleFactor, Time.deltaTime * scaleSpeed);
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, hoverPosition, Time.deltaTime * scaleSpeed);
        }
        else if(!isHovering && !isDragging)
        {
            // Lerp the scale back to original size when not hovering
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originalScale, Time.deltaTime * scaleSpeed);
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, originalPosition, Time.deltaTime * scaleSpeed);
        }
        else if(isDragging){
            Vector2 localMousePosition;
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originalScale * dragScaleFactor, Time.deltaTime * scaleDragSpeed);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localMousePosition);
            rectTransform.anchoredPosition = localMousePosition;  // Set the image position to follow the mouse
        }
    }


    public void OnPointerEnter(PointerEventData eventData){
        isHovering = true;
        rectTransform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData){
        isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData){
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        isDragging = false;
    }
}
