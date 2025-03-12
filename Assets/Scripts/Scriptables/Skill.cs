using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public bool active;
    public int rank;
    public string skillName;
    [TextAreaAttribute] public string skillDescription;
}
