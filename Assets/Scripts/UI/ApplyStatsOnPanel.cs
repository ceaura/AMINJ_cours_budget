using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ApplyStatsOnPanel : MonoBehaviour
{
    [SerializeField] CharacterStats statsToShow;


    [SerializeField, Header("UI")] private TMP_InputField playerNameField;
    [SerializeField] private TextMeshProUGUI lifeDice, life, attack_c, attack_r, attack_m, defence;
    [SerializeField] private TextMeshProUGUI[] stats;
    [SerializeField] private TextMeshProUGUI[] statsMods;
    [SerializeField] private TextMeshProUGUI[] weapons;
    [SerializeField] private TextMeshProUGUI[] armors;
    [SerializeField] private TextMeshProUGUI[] skills;
    [SerializeField] private TextMeshProUGUI[] skillsDescriptions;
    [SerializeField] private TextMeshProUGUI[] equipment;
    [SerializeField] private TMP_InputField playerInputText, characterNameInputText, characterHeightInputText, characterWeigthInputText, characterOldInputText, characterDescInputText;
    [SerializeField] private TMP_Dropdown characterGenderDropdown;


    public void Start()
    {
        Invoke("ApplyStatsToPage", 2);
    }

    public void ApplyStatsToPage()
    {
        playerInputText.text = statsToShow.playerName;
        playerInputText.interactable = false;
        lifeDice.text = statsToShow.profiles.dedevie.ToString();
        life.text = statsToShow.caracteristiques[0].value.ToString();
        defence.text = statsToShow.caracteristiques[1].value.ToString();
        attack_c.text = statsToShow.caracteristiques[2].value.ToString();
        attack_r.text = statsToShow.caracteristiques[3].value.ToString();
        attack_m.text = statsToShow.caracteristiques[4].value.ToString();
        for (int i = 0; i < stats.Length; i++) 
        {
            stats[i].text = statsToShow.caracteristiquesMod[i].value.ToString();
            statsMods[i].text = statsToShow.caracteristiquesMod[i].mod.ToString();
            stats[i].transform.parent.GetComponent<DraggableStat>().ActiveDrag(false);
            Destroy(stats[i].transform.parent.GetComponent<DraggableStat>());
        }
        for (int i = 0; i < statsToShow.profiles.weapons.Length; i++)
        {
            weapons[i].text = statsToShow.profiles.weapons[i].weaponName;
        }
        for (int i = 0; i < statsToShow.profiles.armors.Length; i++)
        {
            armors[i].text = statsToShow.profiles.armors[i].armorName;
        }
        for (int i = 0; i < skills.Length; i++)
        {
            Debug.Log("STATS SKILLS : " + i);
            skills[i].text = statsToShow.profiles.skills[i].skillName;
            skillsDescriptions[i].text = statsToShow.profiles.skills[i].skillDescription;
        }
        for (int i = 0; i < statsToShow.profiles.equipment.Length; i++)
        {
            equipment[i].text = statsToShow.profiles.equipment[i].itemName + " x" + statsToShow.profiles.equipment[i].quantity;
        }
        //characterName, characterGender, characterHeight, characterOld, characterWeigth, characterDescription
        characterNameInputText.text = statsToShow.characterDetails.characterName;
        characterNameInputText.interactable = false;
        Debug.Log(statsToShow.characterDetails.characterGender);
        characterGenderDropdown.value = characterGenderDropdown.options.FindIndex(x => x.text == statsToShow.characterDetails.characterGender);
        characterGenderDropdown.interactable = false;
        characterHeightInputText.text = statsToShow.characterDetails.characterHeight;
        characterHeightInputText.interactable = false;
        characterOldInputText.text = statsToShow.characterDetails.characterOld;
        characterOldInputText.interactable = false;
        characterWeigthInputText.text = statsToShow.characterDetails.characterWeigth;
        characterWeigthInputText.interactable = false;
        characterDescInputText.text = statsToShow.characterDetails.characterDescription;
        characterDescInputText.interactable = false;

    }
}
