using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

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
                Debug.Log("Finit guards");
                break;
            case Zone.Informant:
                Debug.Log("Finit informant");
                break;
            case Zone.Mayor:
                Debug.Log(" Finit Mayor");
                break;
            case Zone.Escape:
                Debug.Log(" FinitEscape");
                break;

            
        }
        return zoneStatuses.ContainsKey(zone) && zoneStatuses[zone].success;
    }
}