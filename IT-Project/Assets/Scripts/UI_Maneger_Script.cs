using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Maneger_Script : MonoBehaviour
{
 
    public GameObject GameOverlayPanel;

    public void Restart()
    {
        Debug.Log("Restarting Test");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOverlayPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
        
    
}
