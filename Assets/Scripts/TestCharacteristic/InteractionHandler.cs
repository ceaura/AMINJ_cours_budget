using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class InteractionHandler : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private InteractionDataSO interactionData;
    [SerializeField] private TextMeshProUGUI resultText;
    

    private int _resultDice;
    private int currentOptionIndex = -1;
    private InteractionOptionSO currentOption;

    private void OnEnable()
    {
        Dice.OnDiceResult += OnDiceResultReceived;
    }

    private void OnDisable()
    {
        Dice.OnDiceResult -= OnDiceResultReceived;
    }

    public void ChooseOption(int index)
    {
        if (index < 0 || index >= interactionData.options.Length)
        {
            Debug.LogWarning("Index d’option invalide !");
            return;
        }

        currentOptionIndex = index;
        currentOption = interactionData.options[currentOptionIndex];

        StartCoroutine(StartTestSequence());
    }

    private void OnDiceResultReceived(int result)
    {
        _resultDice = result;
        StartCoroutine(HandleAfterDice());
    }

    private IEnumerator StartTestSequence()
    {
        yield return Teleport(true);
        DiceThrower.Instance.RollDice();
    }

    private IEnumerator HandleAfterDice()
    {
        yield return new WaitForSeconds(1f);
        HandleDiceResult(_resultDice);
        yield return Teleport(false);
    }

    private void HandleDiceResult(int diceResult)
    {
        if (currentOption == null)
        {
            Debug.LogWarning("Aucune option sélectionnée !");
            return;
        }

        int statValue = GetStatValueFromCharacter(currentOption.characteristic);
        int total = diceResult + statValue;
        bool success = total >= currentOption.characteristicRequirement;

        Debug.Log($"[{interactionData.zone}] Test {currentOption.characteristic} : dé {diceResult} + stat {statValue} = {total} / requis : {currentOption.characteristicRequirement} → {(success ? "Réussi" : "Échoué")}");

        resultText.text = success ? "Vous avez réussi" : "Vous avez échoué"; 
        GameStateManager.Instance.RecordInteraction(interactionData.zone, currentOptionIndex, success);
    }

    private int GetStatValueFromCharacter(CharacterStats.StatsNames statName)
    {
        foreach (var stat in characterStats.caracteristiquesMod)
        {
            if (stat.name == statName)
                return stat.mod;
        }

        foreach (var stat in characterStats.caracteristiques)
        {
            if (stat.name == statName)
                return stat.value + stat.bonus;
        }

        Debug.LogWarning($"Stat {statName} non trouvée !");
        return 0;
    }

    private IEnumerator Teleport(bool toDice)
    {
        //animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);

        if (toDice)
            playerMovement.TeleportToDice();
        else
            playerMovement.TeleportFromDice();

        //animator.SetTrigger("FadeIn");
    }
}
