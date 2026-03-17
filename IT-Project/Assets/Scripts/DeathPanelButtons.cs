using UnityEngine;
using UnityEngine.UI;

public class Panel_Behavior : MonoBehaviour
{
    public Text pointsText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + "Points";
    }
}
