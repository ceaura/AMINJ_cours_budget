using UnityEngine;

[CreateAssetMenu(fileName = "InteractionOption", menuName = "Scriptable Objects/InteractionOption")]
public class InteractionOptionSO : ScriptableObject
{
    public string description;
    public int characteristicRequirement;
    public string characteristicName; 
    public CharacterStats.StatsNames characteristic;
}