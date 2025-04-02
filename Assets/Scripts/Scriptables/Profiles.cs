using UnityEngine;

[CreateAssetMenu(fileName = "Profiles", menuName = "Scriptable Objects/Profiles")]
public class Profiles : ScriptableObject
{
    public int dedevie;
    public Weapon[] weapons;
    public Armor[] armors;
    public Skill[] skills;
    public Item[] equipment;
}
