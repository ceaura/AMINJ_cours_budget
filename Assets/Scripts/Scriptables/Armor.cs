using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Scriptable Objects/Armor")]
public class Armor : ScriptableObject
{
    public string armorName;
    public int armorDefence;
    public int price;
}
