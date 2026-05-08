using UnityEngine;

[CreateAssetMenu(fileName = "Entity_Stats", menuName = "Scriptable Objects/Entity_Stats")]
public class Entity_Stats : ScriptableObject
{
    [SerializeField] public double origin_health;
}
