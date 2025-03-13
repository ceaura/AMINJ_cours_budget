using UnityEngine;
using static CharacterStats;

[CreateAssetMenu(fileName = "Race", menuName = "Scriptable Objects/Race")]
public class Race : ScriptableObject
{
    public CaracteristiqueMod[] caracteristiquesMod = new CaracteristiqueMod[6];
}
