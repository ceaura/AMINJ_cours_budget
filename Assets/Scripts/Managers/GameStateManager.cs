using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [SerializeField] private GameObject GuardsInteraction;
    [SerializeField] private GameObject InformantInteraction;
    [SerializeField] private GameObject MayorInteraction;
    [SerializeField] private GameObject EscapeInteraction;
    [SerializeField] private Animator doorGuardsAnimator;
    
    [SerializeField] private FirstPersonLook camera;
    
    
    [SerializeField] private TextMeshProUGUI forText;
    [SerializeField] private TextMeshProUGUI chaText;
    [SerializeField] private TextMeshProUGUI conText;
    [SerializeField] private TextMeshProUGUI sagText;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private TextMeshProUGUI dexText;
    
    [SerializeField] private CharacterStats stats;

    private InteractionManager interactionManager;

    public enum Zone
    {
        None,
        Guards,
        Informant,
        Mayor,
        Escape
    }

    [System.Serializable]
    public class ZoneStatus
    {
        public bool hasInteracted;
        public int methodUsed = -1;
        public bool success;
    }

    public Zone currentZone = Zone.None;
    private Dictionary<Zone, ZoneStatus> zoneStatuses = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        // Init all zone statuses
        foreach (Zone zone in System.Enum.GetValues(typeof(Zone)))
        {
            if (zone != Zone.None)
                zoneStatuses[zone] = new ZoneStatus();
        }
    }

    private void Start()
    {
        forText.text = stats.caracteristiquesMod[0].value.ToString();
        dexText.text = stats.caracteristiquesMod[1].value.ToString();
        conText.text = stats.caracteristiquesMod[2].value.ToString();
        intText.text = stats.caracteristiquesMod[3].value.ToString();
        sagText.text = stats.caracteristiquesMod[4].value.ToString();
        chaText.text = stats.caracteristiquesMod[5].value.ToString();
    }

    public void SetZone(Zone newZone)
    {
        currentZone = newZone;
    }

    public ZoneStatus GetZoneStatus(Zone zone)
    {
        return zoneStatuses.ContainsKey(zone) ? zoneStatuses[zone] : null;
    }

    public void RecordInteraction(Zone zone, int methodIndex, bool success)
    {
        if (success) HasCompletedZone(zone);
        
        if (!zoneStatuses.ContainsKey(zone)) return;

        zoneStatuses[zone].hasInteracted = true;
        zoneStatuses[zone].methodUsed = methodIndex;
        zoneStatuses[zone].success = success;
    }

    public bool HasCompletedZone(Zone zone)
    {
        switch (zone)
        {
            case Zone.Guards:
                GuardsInteraction.SetActive(false);
                doorGuardsAnimator.SetTrigger("Open");
                interactionManager = GuardsInteraction.GetComponent<InteractionManager>();
                interactionManager.DeleteConservation();
                Debug.Log("Finit guards");
                break;
            case Zone.Informant:
                InformantInteraction.SetActive(false);
                interactionManager = InformantInteraction.GetComponent<InteractionManager>();
                Debug.Log("Finit informant");
                break;
            case Zone.Mayor:
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("MainMenu");
                break;
            case Zone.Escape:
                EscapeInteraction.SetActive(false);
                Debug.Log("Finit Escape");
                break;
        }
        interactionManager.DeleteConservation();
        Cursor.lockState = CursorLockMode.Locked;
        camera.cameraLocked = false;
        return zoneStatuses.ContainsKey(zone) && zoneStatuses[zone].success;
    }
}