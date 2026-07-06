using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    //public TextMeshProUGUI costText;
    public Image artwork;

    private CardData cardData;

    public void Setup(CardData data)
    {
        cardData = data;

        cardNameText.text = data.cardName;
        //costText.text = data.cost.ToString();

        if (data.sprite != null)
            artwork.sprite = data.sprite;
    }

    public void OnCardClicked()
    {
        Debug.Log("Karte gespielt: " + cardData.cardName);

        // Hier Angriff ausführen
    }
}
