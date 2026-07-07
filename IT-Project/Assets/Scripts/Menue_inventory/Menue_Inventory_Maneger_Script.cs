using UnityEngine;
using UnityEngine.SceneManagement;

public class Menue_Inventory_Maneger_Script : MonoBehaviour { 

    public void NewRun()
    {
        SceneManager.LoadScene(3);
    }

    public void MainMenue() 
    {
        SceneManager.LoadScene(0);
    }

    public void Stats()
    {
        SceneManager.LoadScene(5);
    }

}
