using System;
using DialogueEditor;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public KeyCode m_UpKey;
    public KeyCode m_DownKey;
    public KeyCode m_SelectKey;
    public NPCConversation conversation;
    
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;
    
    private bool isConversationFinished = false;
    [SerializeField] private FirstPersonLook camera;

    public InteractionHandler  testCharacteristic;
    private void Update()
    {
        if (ConversationManager.Instance != null && !isConversationFinished)
        {
            UpdateConversationInput();
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !isConversationFinished)
        {
            ConversationManager.Instance.StartConversation(conversation);
            Cursor.lockState = CursorLockMode.None;
            camera.cameraLocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.Locked;
            camera.cameraLocked = false;
        }
    }

    private void UpdateConversationInput()
    {
        if (!ConversationManager.Instance.IsConversationActive) return;

        if (Time.time - lastInputTime < inputCooldown) return; // on attend le cooldown

        if (Input.GetKeyDown(m_UpKey))
        {
            Debug.Log("up key");
            ConversationManager.Instance.SelectPreviousOption();
            lastInputTime = Time.time;
        }
        else if (Input.GetKeyDown(m_DownKey))
        {
            ConversationManager.Instance.SelectNextOption();
            lastInputTime = Time.time;
        }
        else if (Input.GetKeyDown(m_SelectKey))
        {
            ConversationManager.Instance.PressSelectedOption();
            lastInputTime = Time.time;
        }
    }

    public void DeleteConservation()
    {
        isConversationFinished = true;
        ConversationManager.Instance.EndConversation();
    }
}

