using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenueUiManeger : MonoBehaviour
{
    public GameObject Options;
    public GameObject Menue;

    public void OpenOptions()
    {
        Options.SetActive(true);
        Menue.SetActive(false);
    }

    public void OpenMainMenue()
    {
        Options.SetActive(false);
        Menue.SetActive(true);
    }

    public void PlayGame()
    {
        //Später maybe mit save point level ersetzen momentan immer bei level 1 anfangen
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
        //Ist nur für test in editor damit es auch da funktioniert, die line dafor sorgt dafür das es in einer build verion geht
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OpenLevelSelector()
    {
        SceneManager.LoadScene(1);
    }
}
