using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueEditor
{
    public class DialogManager : MonoBehaviour
    {
        public KeyCode m_UpKey;
        public KeyCode m_DownKey;
        public KeyCode m_SelectKey;
        public NPCConversation conversation;

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
}
