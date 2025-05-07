using System;
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

    [SerializeField] private AudioClip[] audioClip;
    private AudioSource audioSource;

    private int _resultDice;
    private int currentOptionIndex = -1;
    private InteractionOptionSO currentOption;

    private enum Phase { Idle, WaitingForDice, WaitAfterDice }
    private Phase currentPhase = Phase.Idle;
    private float phaseTimer = 0f;

    private void OnEnable()
    {
        Dice.OnDiceResult += OnDiceResultReceived;
    }

    private void OnDisable()
    {
        Dice.OnDiceResult -= OnDiceResultReceived;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        playerMovement.TeleportToDice();
        DiceThrower.Instance.RollDice();
        currentPhase = Phase.WaitingForDice;
    }

    private void OnDiceResultReceived(int result)
    {
        _resultDice = result;
        phaseTimer = 1f; // délai d'attente avant traitement
        currentPhase = Phase.WaitAfterDice;
    }

    private void Update()
    {
        if (currentPhase == Phase.WaitAfterDice)
        {
            phaseTimer -= Time.deltaTime;
            if (phaseTimer <= 0f)
            {
                HandleDiceResult(_resultDice);
                playerMovement.TeleportFromDice();
                currentPhase = Phase.Idle;
            }
        }
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

        if (success)
        {
            resultText.text = "Le test est réussi, vous pouvez avancer !" ;
            audioSource.clip = audioClip[0];
        }
        else
        {
            resultText.text = "Le test a échoué, vous devez recommencer" ;
            audioSource.clip = audioClip[1];
        }

        audioSource.Play();
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
}

