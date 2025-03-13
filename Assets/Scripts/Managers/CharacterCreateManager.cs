using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCreateManager : MonoBehaviour
{
    [SerializeField, Header("Stats")] private CharacterStats _characterStats;
    [SerializeField] private List<Profiles> _warrior = new(), _magician = new(), _priest = new(), _prowler = new(), _thief = new();
    [SerializeField] private List<Race> _half_orc = new(), _half_elf = new(), _elf = new(), _dwarf = new(), _human = new();
  
    [SerializeField, Header("UI")] private TMP_InputField playerNameField;
    [SerializeField] private TextMeshProUGUI[] carasTexts, modsTexts;

    // Nombre total de charactéristiques des personnages.
    private int caractnum = 6;
    private int[] mods = new int[21] { -4, -4, -4, -3, -3, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };


    private void Start()
    {
        // Entrer le nom du joueur.
        playerNameField.onSubmit.AddListener(delegate { SetPlayerName(playerNameField.text); });
        (int, int)[] caracsAndMods = LaunchDices();
        for (int i = 0; i < caracsAndMods.Length; i++)
        {
            carasTexts[i].text = caracsAndMods[i].Item1.ToString();
            modsTexts[i].text = (caracsAndMods[i].Item2 > 0 ? "+" : "") + caracsAndMods[i].Item2.ToString();
        }
    }

    // Appliquer le nom du joueur.
    private void SetPlayerName(string pName)
    {
        _characterStats.playerName = pName;
    }

    // Lancer de dé
    private (int, int)[] LaunchDices()
    {
        int modsResult = 0;
        (int, int)[] caracsAndMods = new (int, int)[caractnum];
        for (int i = 0; i < caractnum; i++) {
            int defdice = 6;
            int dice1 = Random.Range(1, 7);
            if (dice1 < defdice) defdice = dice1;
            int dice2 = Random.Range(1, 7);
            if (dice2 < defdice) defdice = dice2;
            int dice3 = Random.Range(1, 7);
            if (dice3 < defdice) defdice = dice3;
            int dice4 = Random.Range(1, 7);
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
