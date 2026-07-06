using UnityEngine;

public class HandFocusManager : MonoBehaviour
{
    [Header("Referenzen")]
    public Transform player;
    public RectTransform cardContainer;

    [Header("Idle Einstellungen")]
    public float idleTimeRequired = 3f;

    [Header("Skalierung")]
    public float smallScale = 0.8f;
    public float largeScale = 1.2f;

    [Header("Position")]
    public float loweredY = 80f;
    public float raisedY = 160f;

    [Header("Animation")]
    public float animationSpeed = 5f;

    private Vector3 lastPosition;
    private float idleTimer;

    private void Start()
    {
        lastPosition = player.position;
    }

    private void Update()
    {
        bool isMoving =
            Vector3.Distance(
                player.position,
                lastPosition) > 0.01f;

        float targetScale;
        float targetY;

        if (isMoving)
        {
            idleTimer = 0f;

            targetScale = smallScale;
            targetY = loweredY;
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimeRequired)
            {
                targetScale = largeScale;
                targetY = raisedY;
            }
            else
            {
                targetScale = smallScale;
                targetY = loweredY;
            }
        }

        cardContainer.localScale =
            Vector3.Lerp(
                cardContainer.localScale,
                Vector3.one * targetScale,
                Time.deltaTime * animationSpeed);

        Vector2 currentPos =
            cardContainer.anchoredPosition;

        currentPos.y = Mathf.Lerp(
            currentPos.y,
            targetY,
            Time.deltaTime * animationSpeed);

        cardContainer.anchoredPosition =
            currentPos;

        lastPosition = player.position;
    }
}