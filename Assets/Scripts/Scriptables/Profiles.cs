using UnityEngine;

[CreateAssetMenu(fileName = "Profiles", menuName = "Scriptable Objects/Profiles")]
public class Profiles : ScriptableObject
{
    public int dedevie;
    public Weapon[] armes;
    public Armor[] armures;
    public Skill[] capacites;
    public Item[] equipements;
}
