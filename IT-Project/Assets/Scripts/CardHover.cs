using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private Vector3 originalScale;
    private Vector3 hoverScale;

    private void Start()
    {
        originalScale = transform.localScale;
        hoverScale = originalScale * 1.2f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
