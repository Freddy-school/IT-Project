using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Maneger_Script : MonoBehaviour
{
 
    public GameObject GameOverlayPanel;
    public GameObject GameOverPanel;
    public GameObject OptionsPanel;

    public void Restart()
    {
        Debug.Log("Restarting Test");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOverPanel.SetActive(false);
        GameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void GoMainMenue()
    {
        Debug.Log("Test Go mainMenue");
        GameOverlayPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);

    }

    public void ReturnToGame()
    {
        Debug.Log("ReturningToGame");
        GameOverlayPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

    }

    public void OpenOptions()
    {
        Debug.Log("ReturningToGame");
        GameOverlayPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        OptionsPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

    }


}
