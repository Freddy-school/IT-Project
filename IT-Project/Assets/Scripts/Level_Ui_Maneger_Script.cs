using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Ui_Maneger_Script : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene(6);
    }

    public void OpenMainMenue()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenDungeon()
    {
        SceneManager.LoadScene(3);
    }
}
