using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonExit : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName;


    private bool loading = false;


    private void OnTriggerEnter(Collider other)
    {
        if (loading)
            return;


        if (other.CompareTag("Charakter_Player"))
        {
            loading = true;

            Debug.Log("Exit erreicht - Lade nächste Szene");


            SceneManager.LoadScene(4);
        }
    }
}