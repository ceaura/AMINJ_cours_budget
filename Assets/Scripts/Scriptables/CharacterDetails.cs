using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDetails", menuName = "Scriptable Objects/CharacterDetails")]
public class CharacterDetails : ScriptableObject
{
    public string characterName, characterGender, characterHeight, characterOld, characterWeigth, characterDescription;
}
