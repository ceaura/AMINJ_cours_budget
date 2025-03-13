using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string playerName;
    public enum StatsNames
    {
        FOR, DEX, CON, INT, SAG, CHA, PV, DEF, DMG_Contact, DMG_Distance, DMG_Magique
    }
    [System.Serializable]
    public class Caracteristique
    {
        public StatsNames name;
        public int value;
        public int bonus;
    }

    [System.Serializable]
    public class CaracteristiqueMod : Caracteristique
    {
        public int mod;
    }

    public int niveau;
    public CaracteristiqueMod[] caracteristiquesMod = new CaracteristiqueMod[6];
    public Caracteristique[] caracteristiques = new Caracteristique[5];
    public Race race;
    public Profiles profiles;
}
