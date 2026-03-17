using UnityEngine;

public class Game_Controller_Script : MonoBehaviour
{
    [SerializeField] public GameObject Enemy1_Prefap;

    void Start()
    {
        Instantiate(Enemy1_Prefap, new Vector3(3, 2,-4), Quaternion.identity); 
    }
}
