using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    [Header("Referenzen")]
    public Transform cardContainer;
    public GameObject cardPrefab;

    [Header("Handkarten")]
    public List<CardData> cardsInHand = new();

    [Header("Layout")]
    public float cardSpacing = 150f;
    public float fanAngle = 30f;
    public float curveHeight = 50f;

    private void Start()
    {
        RefreshHand();
        Debug.Log("Hotbar gestartet");
    }

    public void RefreshHand()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (CardData card in cardsInHand)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);

            CardUI cardUI = cardObj.GetComponent<CardUI>();
            cardUI.Setup(card);
        }

        ArrangeCards();
        
        Debug.Log("Karten in Hand: " + cardsInHand.Count);
    }

    private void ArrangeCards()
    {
        int count = cardContainer.childCount;

        if (count == 0)
            return;

        float startX = -((count - 1) * cardSpacing) / 2f;

        for (int i = 0; i < count; i++)
        {
            RectTransform card =
                cardContainer.GetChild(i).GetComponent<RectTransform>();

            float normalized = count == 1
                ? 0.5f
                : (float)i / (count - 1);

            float angle = Mathf.Lerp(
                -fanAngle / 2f,
                fanAngle / 2f,
                normalized);

            float xPos = startX + i * cardSpacing;

            float distanceFromCenter =
                Mathf.Abs(normalized - 0.5f) * 2f;

            float yPos =
                -distanceFromCenter * curveHeight;

            card.anchoredPosition =
                new Vector2(xPos, yPos);


            card.localRotation =
                Quaternion.Euler(
                    12f,
                    0f,
                    angle);

        }
    }
}
