using System;
using DialogueEditor;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(InteractionManager))]
public class InteractionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InteractionManager dialogManager = (InteractionManager)target;
        if (GUILayout.Button("Démarrer la conversation"))
        {
            if (ConversationManager.Instance != null)
                ConversationManager.Instance.StartConversation(dialogManager.conversation);
            else
                Debug.LogWarning("ConversationManager.Instance est null !");
        }
    }
}
public class InteractionManager : MonoBehaviour
{
    public KeyCode m_UpKey;
    public KeyCode m_DownKey;
    public KeyCode m_SelectKey;
    public NPCConversation conversation;

    public InteractionHandler  testCharacteristic;
    private void Update()
    {
        if (ConversationManager.Instance != null)
        {
            UpdateConversationInput();
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    private void UpdateConversationInput()
    {
        if (ConversationManager.Instance.IsConversationActive)
        {
            if (Input.GetKeyDown(m_UpKey))
                ConversationManager.Instance.SelectPreviousOption();
            else if (Input.GetKeyDown(m_DownKey))
                ConversationManager.Instance.SelectNextOption();
            else if (Input.GetKeyDown(m_SelectKey))
                ConversationManager.Instance.PressSelectedOption();
        }
    }
}

