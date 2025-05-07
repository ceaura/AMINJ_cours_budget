using UnityEngine;

[CreateAssetMenu(fileName = "TestCharacteristicSO", menuName = "Scriptable Objects/TestCharacteristicSO")]
public class TestCharacteristicSO : ScriptableObject
{
    public string testName;
    public string testDescription;
    public int threshold; 
    public CharacterStats.StatsNames characteristic;
    
}
