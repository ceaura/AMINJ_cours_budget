using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData", menuName = "Scriptable Objects/InteractionData")]
public class InteractionDataSO : ScriptableObject
{
    public GameStateManager.Zone zone;
    public string interactionName;
    public InteractionOptionSO[] options; 
}

