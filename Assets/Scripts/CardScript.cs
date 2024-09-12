using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Canvas canvas;
    public GrilsManagerScript girlsManager;
    private bool isHovering = false;
    private bool isDragging = false;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    public Vector3 originalPosition;

    private float hoverScaleFactor = 1.7f;
    private float dragScaleFactor = 0.3f;
    private Vector3 hoverPosition;
    private float scaleSpeed = 5f;
    private float scaleDragSpeed = 10f;

    //Card effects
    public string effect;
    public float amount;
    public bool isRandom = false;
    public float secondAmount;
    public bool twoEffects = false;
    public string secondEffect;
    public float amount2;
    public float agresivity;


    void Awake(){
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        hoverPosition = originalPosition + new Vector3(0, 70, 0);
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out localMousePosition);
            float canvasHeight = (canvas.transform as RectTransform).rect.height;
            localMousePosition.y += canvasHeight/2;
            rectTransform.anchoredPosition = localMousePosition;  // Set the image position to follow the mouse

            girlsManager.HighlightCharacter(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }


    public void OnPointerEnter(PointerEventData eventData){
        isHovering = true;
        hoverPosition = originalPosition + new Vector3(0, 70, 0);
        rectTransform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData){
        isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData){
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        bool isUsed;
        if(isRandom){
            isUsed = girlsManager.CardUsed(effect, amount, secondAmount, agresivity);
        }else{
            isUsed = girlsManager.CardUsed(effect, amount, agresivity);
        }
        if(twoEffects){
            girlsManager.CardUsed(secondEffect, amount, 0);
        }
        
        if(isUsed){
            gameObject.SetActive(false);
        }
        isDragging = false;
    }
}
