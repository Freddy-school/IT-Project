using UnityEngine;

public class CardSelectionManager : MonoBehaviour
{
    public Transform cardContainer;

    public float raisedHeight = 40f;

    private int selectedIndex = -1;

    public Player_Controller player;

    public PlayerCombat playerCombat;
    
    public HotbarManager hotBarManager;

    public Player_Attacks playerAttacks;


    private void Update()
    {
        CheckInput();

        UpdateCardPositions();


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Linksklick erkannt");
            PlaySelectedCard();

            Debug.Log("PlaySelectedCard gestartet");
            Debug.Log("Selected Index: " + selectedIndex);

        }
        

    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedIndex = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedIndex = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedIndex = 2;

        if (Input.GetKeyDown(KeyCode.Alpha4))
            selectedIndex = 3;

        if (Input.GetKeyDown(KeyCode.Alpha5))
            selectedIndex = 4;
    }

    private void UpdateCardPositions()
    {
        for (int i = 0; i < cardContainer.childCount; i++)
        {
            RectTransform card =
                cardContainer.GetChild(i)
                .GetComponent<RectTransform>();

            Vector2 targetPos =
                new Vector2(
                    card.anchoredPosition.x,
                    i == selectedIndex
                        ? 40f
                        : 0f);

            card.anchoredPosition =
                Vector2.Lerp(
                    card.anchoredPosition,
                    targetPos,
                    Time.deltaTime * 8f);
        }
    }


    private void PlaySelectedCard()
    {
        if (selectedIndex < 0)
            return;

        CardData selectedCard =
            hotBarManager.cardsInHand[selectedIndex];
        Debug.Log(selectedCard.cardName);

        string attackName =
            selectedCard.cardName;

        switch (attackName)
        {
            case "Combo1":
                playerAttacks.Combo1(selectedCard);
                break;

            case "Combo2":
                playerAttacks.Combo2(selectedCard);
                break;
        }
    }


}

