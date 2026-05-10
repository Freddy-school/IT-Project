using UnityEngine;

public enum EntityType
{
    None,
    Player,
    Enemy1,
    Enemy2
}

[CreateAssetMenu(fileName = "Entity_Stats", menuName = "Scriptable Objects/Entity_Stats")]
public class Entity_Stats : ScriptableObject
{
    [SerializeField] public double origin_health;
    [SerializeField] public EntityType type = EntityType.None;
}
