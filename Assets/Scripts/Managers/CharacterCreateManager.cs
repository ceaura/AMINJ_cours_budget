using Koboct.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CharacterStats;
using static UnityEngine.Rendering.DebugUI;

public class CharacterCreateManager : MonoBehaviour
{
    [SerializeField] private DialogueSystem startDialog;
    [SerializeField, Header("Stats")] private CharacterStats _characterStats;
    [SerializeField] private List<Profiles> classes = new();
    [SerializeField] private List<Race> races = new();

    [SerializeField, Header("UI")] private TMP_InputField playerNameField;
    [SerializeField] private TextMeshProUGUI lifeDice, life, attack_c, attack_r, attack_m, defence;
    [SerializeField] private Transform[] caracSlots;
    [SerializeField] private TextMeshProUGUI[] weaponSlots;
    [SerializeField] private TextMeshProUGUI[] armorSlots;
    [SerializeField] private GameObject skillOverviewArea;
    [SerializeField] private TextMeshProUGUI[] skillNameOverviewSlots;
    [SerializeField] private TextMeshProUGUI[] skillDescriptionOverviewSlots;
    [SerializeField] private TextMeshProUGUI[] skillNameSlots;
    [SerializeField] private TextMeshProUGUI[] skillDescriptionSlots;
    [SerializeField] private TextMeshProUGUI[] equipmentSlots;
    [SerializeField] private TMP_InputField characterNameInputText, characterHeightInputText, characterWeigthInputText, characterOldInputText, characterDescInputText;
    [SerializeField] private TMP_Dropdown characterGenderDropdown;


    private TextMeshProUGUI[] carasTexts = new TextMeshProUGUI[6], modsTexts = new TextMeshProUGUI[6];

    // Nombre total de charactéristiques des personnages.
    private int caractnum = 6;
    private int[] mods = new int[21] { -4, -4, -4, -3, -3, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
    private int selectedRaceId = 0;
    private (int, int)[] caracsAndModsSetter;
    private bool firstSkillSet = false;


    private void Start()
    {
        carasTexts = new TextMeshProUGUI[caractnum];
        modsTexts = new TextMeshProUGUI[caractnum];
        startDialog.StartDialog();
        // Entrer le nom du joueur.
        playerNameField.onSubmit.AddListener(delegate { SetPlayerName(); });
    }

    public void CreatePlayerStats()
    {
        SetCarasTexts();
        caracsAndModsSetter = LaunchDices();
        for (int i = 0; i < caracsAndModsSetter.Length; i++)
        {
            print("slot : " + i);
            ApplyStatsToUi(i, caracsAndModsSetter[i].Item1, caracsAndModsSetter[i].Item2);
        }
    }

    private void SetCarasTexts()
    {
        for (int i = 0; i < caracSlots.Length; i++)
        {
            carasTexts[i] = caracSlots[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            modsTexts[i] = caracSlots[i].GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
            print("HEEEERE " + carasTexts[i].text);
        }
    }

    private void ApplyStatsPreviewToUi(int uiId, int value, int mod)
    {
        print("mod value :" + (caracsAndModsSetter[uiId].Item1 + value - 1));
        int nmod = mods[caracsAndModsSetter[uiId].Item1 + value-1];
        carasTexts[uiId].text += (value >= 0 ? "<color=#00ff00>+" + value + "</color>" : "<color=#ff0000>" + value + "</color>");
        modsTexts[uiId].text += (nmod >= 0 ? "<color=#00ff00>+" + nmod + "</color>" : "<color=#ff0000>" + nmod + "</color>");
    }

    private void ApplyStatsToUi(int uiId, int value, int mod)
    {
        carasTexts[uiId].text = value.ToString();
        modsTexts[uiId].text = (mod > 0 ? "+" : "") + mod.ToString();
    }

    // Appliquer le nom du joueur.
    public void SetPlayerName()
    {
        if (playerNameField.text.Length < 1) { startDialog.WriteText(3);
        }
        else
        {
            _characterStats.playerName = playerNameField.text;
            startDialog.WriteText(4);
        }
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

    public void ConfirmCaracs()
    {
        SetCarasTexts();
        for (int i = 0; i < caracSlots.Length; i++)
        {
            caracsAndModsSetter[i].Item1 = int.Parse(carasTexts[i].text);
            caracsAndModsSetter[i].Item2 = int.Parse(modsTexts[i].text);
            caracSlots[i].GetChild(0).GetComponent<DraggableStat>().ActiveDrag(false);
            Destroy(caracSlots[i].GetChild(0).GetComponent<DraggableStat>());
        }
    }

    // Choix de la race. (change des stats) 0 = _half_orc , 1 = _half_elf, 2 = _elf, 3 = _dwarf, 4 = _human;
    public void PreviewRace(int raceId)
    {
        selectedRaceId = raceId;
        Race SelectedRace = races[raceId];
        for (int i = 0; i < SelectedRace.caracteristiquesMod.Length; i++)
        {
            CaracteristiqueMod caraM = SelectedRace.caracteristiquesMod[i];
            ApplyStatsToUi(i, caracsAndModsSetter[i].Item1 + caraM.value, mods[caracsAndModsSetter[i].Item1 + caraM.mod - 1]);
            ApplyStatsPreviewToUi(i, caraM.value, caraM.mod);
        }
    }

    public void ConfirmRace()
    {
        Race SelectedRace = races[selectedRaceId];
        for (int i = 0; i < SelectedRace.caracteristiquesMod.Length; i++)
        {
            CaracteristiqueMod caraM = SelectedRace.caracteristiquesMod[i];
            caracsAndModsSetter[i] = (caracsAndModsSetter[i].Item1+caraM.value, caracsAndModsSetter[i].Item2 + caraM.mod);
            ApplyStatsToUi(i, caracsAndModsSetter[i].Item1, caracsAndModsSetter[i].Item2);
        }
        _characterStats.race = SelectedRace;
        for (int i = 0; i < _characterStats.caracteristiquesMod.Length;i++)
        {
            _characterStats.caracteristiquesMod[i].value = caracsAndModsSetter[i].Item1;
            _characterStats.caracteristiquesMod[i].mod = caracsAndModsSetter[i].Item2;
        }
    }

    // Choix du profil (vie, armes, armures et items) _warrior, _magician, _priest, _prowler, _thief
    public void PreviewClass(int raceId)
    {
        skillOverviewArea.SetActive(true);
        lifeDice.text = classes[raceId].dedevie.ToString();
        _characterStats.profiles = classes[raceId];
        //equipements, weapons, armors, and skills
        for (int i = 0; i < equipmentSlots.Length; i++) {
            if (classes[raceId].equipment.Length > i)
                equipmentSlots[i].text = classes[raceId].equipment[i].itemName + " x" + classes[raceId].equipment[i].quantity;
            else equipmentSlots[i].text = "";
        }
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (classes[raceId].weapons.Length > i)
                weaponSlots[i].text = classes[raceId].weapons[i].weaponName;
            else weaponSlots[i].text = "";
        }
        for (int i = 0; i < armorSlots.Length; i++)
        {
            if (classes[raceId].armors.Length > i)
                armorSlots[i].text = classes[raceId].armors[i].armorName;
            else armorSlots[i].text = "";
        }
        for (int i = 0; i < skillNameOverviewSlots.Length; i++)
        {
            if (classes[raceId].skills.Length > i)
            {
                skillNameOverviewSlots[i].text = classes[raceId].skills[i].skillName;
                skillDescriptionOverviewSlots[i].text = classes[raceId].skills[i].skillDescription;
            }
            else
            {
                skillNameOverviewSlots[i].text = "";
                skillDescriptionOverviewSlots[i].text = "";
            }
        }
    }

    // Choix des capacités
    public void SetSkill(int skillValue)
    {
        if (!firstSkillSet)
        {
            if (!skillNameSlots[1].text.Contains(_characterStats.profiles.skills[skillValue].skillName))
            {
                skillNameSlots[0].text = _characterStats.profiles.skills[skillValue].skillName;
                skillDescriptionSlots[0].text = _characterStats.profiles.skills[skillValue].skillDescription;
                firstSkillSet = !firstSkillSet;
            }
        }
        else
        {
            if (!skillNameSlots[0].text.Contains(_characterStats.profiles.skills[skillValue].skillName))
            {
                skillNameSlots[1].text = _characterStats.profiles.skills[skillValue].skillName;
                skillDescriptionSlots[1].text = _characterStats.profiles.skills[skillValue].skillDescription;
                firstSkillSet = !firstSkillSet;
            }
        }
    }
    public void ConfirmSkill()
    {
        for (int i = 0; i < skillNameSlots.Length; i++)
        {
            for(int j = 0; j < _characterStats.profiles.skills.Length; j++)
            {
                if (skillNameSlots[i].text.Contains(_characterStats.profiles.skills[j].name)) 
                {
                    _characterStats.profiles.skills[j].active = true;
                }
            }
        }
        // PV = dé de vie (DV) + Mod. de CON
        _characterStats.caracteristiques[0].value = Random.Range(1, _characterStats.profiles.dedevie+1) + _characterStats.caracteristiquesMod[2].mod;
        life.text = _characterStats.caracteristiques[0].value.ToString();
        // Attaque c = Mod. de FOR +1,
        _characterStats.caracteristiques[2].value = _characterStats.caracteristiquesMod[0].mod+1;
        attack_c.text = _characterStats.caracteristiques[2].value.ToString();
        // r = Mod. de DEX + 1,
        _characterStats.caracteristiques[3].value = _characterStats.caracteristiquesMod[1].mod+1;
        attack_r.text = _characterStats.caracteristiques[3].value.ToString();
        // m = Mod. d’INT + 1 (mage) ou Mod. de SAG + 1 (prêtre)
        _characterStats.caracteristiques[4].value = _characterStats.profiles.name.Contains("Magician") ? _characterStats.caracteristiquesMod[3].mod + 1 : _characterStats.profiles.name.Contains("Priest") ? _characterStats.caracteristiquesMod[4].mod + 1 : 0;
        attack_m.text = _characterStats.caracteristiques[4].value.ToString();
        // Defense = 10 + Mod. de DEX + Mod. d’armure + Mod.de bouclier
        _characterStats.caracteristiques[1].value = _characterStats.caracteristiquesMod[1].mod + _characterStats.profiles.armors[0].armorDefence;
        if (_characterStats.profiles.armors.Length > 1) _characterStats.caracteristiques[1].value += _characterStats.profiles.armors[1].armorDefence;
        defence.text = _characterStats.caracteristiques[1].value.ToString();
    }


    // Remplir les détails du personnage (nom, sexe age, taille, poids, Description).
    public void SetCharacterDetails()
    {
        Debug.Log("SET DETAILS");
        _characterStats.characterDetails.characterName = characterNameInputText.text;
        _characterStats.characterDetails.characterGender = characterGenderDropdown.value.ToString();
        _characterStats.characterDetails.characterHeight = characterHeightInputText.text;
        _characterStats.characterDetails.characterWeigth = characterWeigthInputText.text;
        _characterStats.characterDetails.characterOld = characterOldInputText.text;
        _characterStats.characterDetails.characterDescription = characterDescInputText.text;
        _characterStats.level = 1;
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
