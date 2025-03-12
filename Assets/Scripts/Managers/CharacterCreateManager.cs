using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreateManager : MonoBehaviour
{
    [SerializeField, Header("Stats")] private CharacterStats _characterStats;
    [SerializeField] private List<Weapon> _warriorWeapons = new(), _magicianWeapons = new(), _priestWeapons = new(), _prowlerWeapons = new(), _thiefWeapons = new();
    [SerializeField] private List<Armor> _warriorArmors = new(), _magicianArmors = new(), _priestArmors = new(), _prowlerArmors = new(), _thiefArmors = new();
    [SerializeField] private List<Item> _warriorItems = new(), _magicianItems = new(), _priestItems = new(), _prowlerItems = new(), _thiefItems = new();
    [SerializeField] private List<Skill> _warriorSkills = new(), _magicianSkills = new(), _priestSkills = new(), _prowlerSkills = new(), _thiefSkills = new();

    [SerializeField, Header("UI")] private InputField playerNameField;

    private int caractnum = 6;
    private int[] mods = new int[21] { -4, -4, -4, -3, -3, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };


    private void Start()
    {
        playerNameField.onSubmit.AddListener(delegate { SetPlayerName(playerNameField.text); });
        (int, int)[] caracsAndMods = LaunchDices();
    }

    // Entrer le nom du joueur
    private void SetPlayerName(string pName)
    {
        _characterStats.playerName = pName;
    }

    // Lancer de dé
    private (int, int)[] LaunchDices()
    {
        // Nombre total de charactéristiques des personnages.
        int modsResult = 0;
        (int, int)[] caracsAndMods = new (int, int)[caractnum];
        for (int i = 0; i < caractnum; i++) {
            int defdice = 6;
            int dice1 = Random.Range(1, 6);
            if (dice1 < defdice) defdice = dice1;
            int dice2 = Random.Range(1, 6);
            if (dice2 < defdice) defdice = dice2;
            int dice3 = Random.Range(1, 6);
            if (dice3 < defdice) defdice = dice3;
            int dice4 = Random.Range(1, 6);
            if (dice4 < defdice) defdice = dice4;

            int stat = dice1 + dice2 + dice3 + dice4 - defdice;
            caracsAndMods[i].Item1 = stat;
            caracsAndMods[i].Item2 = mods[stat-1];
            modsResult += mods[stat - 1];
            print(" Char" + i + " " + caracsAndMods[i].Item1 + " Mod " + caracsAndMods[i].Item2);
        }
        print(" Mod Result : " + modsResult);
        if (modsResult < 3)
        {
            return LaunchDices();
        }
        return caracsAndMods;
    }

    // Choix de la race. (change des stats)

    // Choix du profil (vie, armes, armures et items)
    // Choix des capacités
    // Defense
    // Attaque

    // Remplir les détails du personnage (nom, sexe age, taille, poids, Description).




}
