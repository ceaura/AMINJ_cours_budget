using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string playerName;
    public int niveau, dedevie,
            FOR, DEX, CON, INT, SAG, CHA,
            FOR_mod, DEX_mod, CON_mod, INT_mod, SAG_mod, CHA_mod,
            FOR_bon, DEX_bon, CON_bon, INT_bon, SAG_bon, CHA_bon,
            PV, PV_bon, PV_rest, DEF, DEF_bon,
            DMG_Contact, DMG_Contact_Bon,
            DMG_Distance, DMG_Distanc_Bon,
            DMG_Magique, DMG_Magique_Bon;

    public Weapon[] armes = new Weapon[2];
    public Armor[] armures = new Armor[2];
    public Skill[] capacites = new Skill[12];
    public Item[] equipements = new Item[12];

}
