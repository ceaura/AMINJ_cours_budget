using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Image dialogUi;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private VerticalLayoutGroup choiceParent;
    [SerializeField, Tooltip("characters per seconds")] private float speechSpeed = 40;
    [SerializeField] private float dialogMaxAlphaColor = 0.65f;

    [SerializeField] private UnityEvent endDialEvent;

    private Button[] choicesButtons;
    private int dialId = 0;

    [System.Serializable]
    private class Choice
    {
        public string choice;
        public int nextDialId;
        public UnityEvent choiceEvent;
    }
    [System.Serializable]
    private class Dialogue
    {
        public bool endDialog = false;
        public string text;
        public Choice[] choices;
        public bool multiplechoice = false;
        public UnityEvent dialEvent;
    }

    [SerializeField] private Dialogue[] dials = new Dialogue[]
    {
        new Dialogue { text = "Bonjour joueur, bienvenue dans Clairval."},
        new Dialogue { text = "Avant d'entré dans l'aventure, créons votre personnage."},
        new Dialogue { text = "Entrez votre nom de joueur dans la fiche située à votre droite !", choices = new Choice[]
            {
                new Choice { choice = "suivant", nextDialId = -1 },
            }},
        new Dialogue { text = "Le nom entré n'est pas valide", choices = new Choice[]
            {
                new Choice { choice = "retour", nextDialId = 2 },
            }},
        new Dialogue { text = "Des statistiques vous ont été attribuée, vous pouvez les changer de place en les glissant sur la caractéristique voulu.", choices = new Choice[]
            {
                new Choice { choice = "Mes caractéristiques me conviennent.", nextDialId = 5 },
            }},
        new Dialogue { text = "Confirmer les caractéristiques ? (vous ne pourrez plus les déplacer).", choices = new Choice[]
            {
                new Choice { choice = "Oui", nextDialId = 6 },
                new Choice { choice = "Non", nextDialId = 4 },
            }},
        new Dialogue { text = "Choisissez une race", choices = new Choice[]
            {
                new Choice { choice = "Humain", nextDialId = 7 },
                new Choice { choice = "Elfe", nextDialId = 7 },
                new Choice { choice = "Demi-Elfe", nextDialId = 7 },
                new Choice { choice = "Nain", nextDialId = 7 },
                new Choice { choice = "Demi-Orque", nextDialId = 7 },
            }},
        new Dialogue { text = "Confirmer la race ? (vous ne pourrez plus la changer).", choices = new Choice[]
            {
                new Choice { choice = "Oui", nextDialId = 8 },
                new Choice { choice = "Non", nextDialId = 6 },
            }},
        new Dialogue { text = "Choisissez une classe", choices = new Choice[]
            {
                new Choice { choice = "Guerrier", nextDialId = 9 },
                new Choice { choice = "Mage", nextDialId = 9 },
                new Choice { choice = "Voleur", nextDialId = 9 },
                new Choice { choice = "Prêtre", nextDialId = 9 },
                new Choice { choice = "Rôdeur", nextDialId = 9 },
            }},
        new Dialogue { text = "Confirmer la classe ? (vous ne pourrez plus la changer).", choices = new Choice[]
            {
                new Choice { choice = "Oui", nextDialId = 10 },
                new Choice { choice = "Non", nextDialId = 8 },
            }},
        new Dialogue { text = "Sur la feuille, choisissez 2 des 3 capacités de rang 1.", choices = new Choice[]
            {
                new Choice { choice = "Suivant", nextDialId = 8 },
            }},
        new Dialogue { text = "Félicitation, vous avez créé votre personnage, vous pouvez maintenant partir à l'aventure !", choices = new Choice[]
            {
                new Choice { choice = "Super !", nextDialId = 8 },
            }},
    };
    private void Start()
    {
        dialogUi.color = new Color(dialogUi.color.r, dialogUi.color.g, dialogUi.color.b, 0);
        choicesButtons = new Button[choiceParent.transform.childCount];
        for (int i = 0; i < choiceParent.transform.childCount; i++) {
            choicesButtons[i] = choiceParent.transform.GetChild(i).GetComponent<Button>();
        }
        choiceParent.gameObject.SetActive(false);
    }

    public void StartDialog()
    {
        print("DIALOGUE STARTED");
        StartCoroutine(FadeInDialog());
    }

    private IEnumerator FadeInDialog()
    {
        float startTime = Time.time;
        while (dialogUi.color.a < dialogMaxAlphaColor) 
        { 
            yield return new WaitForSeconds(Time.time - startTime);
            dialogUi.color = new Color(dialogUi.color.r, dialogUi.color.g, dialogUi.color.b, dialogUi.color.a + (Time.time - startTime) * dialogMaxAlphaColor);
            startTime = Time.time;
        }
        WriteText(0);
    }

    private IEnumerator FadeOutDialog()
    {
        float startTime = Time.time;
        while (dialogUi.color.a > 0)
        {
            yield return new WaitForSeconds(Time.time - startTime);
            dialogUi.color = new Color(dialogUi.color.r, dialogUi.color.g, dialogUi.color.b, dialogUi.color.a - (Time.time - startTime) * dialogMaxAlphaColor);
            startTime = Time.time;
        }
    }

    public void WriteText (int dialIdWrite)
    {
        dialId = dialIdWrite;
        StartCoroutine(WriteText(dials[dialId].text));
    }

    private IEnumerator WriteText(string text)
    {
        int length = 1;
        dialogText.text = "";
        while (dialogText.text.Length < text.Length)
        {
            yield return new WaitForSeconds(1/speechSpeed);
            dialogText.text = text[..length];
            length += 1;
        }
        if (dials[dialId].choices.Length > 0)
        {
            choiceParent.gameObject.SetActive(true);
            for (int i = 0; i < dials[dialId].choices.Length; i++)
            {
                int choiceIndex = i;
                choicesButtons[i].onClick.RemoveAllListeners();
                choicesButtons[i].onClick.AddListener(delegate { OnChoiceSelected(dials[dialId].multiplechoice, dials[dialId].choices[choiceIndex]);});
                choicesButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dials[dialId].choices[i].choice;
                choicesButtons[i].gameObject.SetActive(true);
            }
            yield break;
        }
        dials[dialId].dialEvent.Invoke();
        yield return new WaitForSeconds(1);
        dialId++;
        print("DIALOGUE FINISHED : " + dialId + " / " + dials.Length);
        if (dialId < dials.Length && dials[dialId-1].endDialog == false)
        {
            StartCoroutine(WriteText(dials[dialId].text));
        }
        else
        {
            dialogText.text = "";
            StartCoroutine(FadeOutDialog());
            endDialEvent.Invoke();
        }
    }

    private void OnChoiceSelected(bool multiplechoice, Choice choice)
    {
        if (!multiplechoice)
        {
            choice.choiceEvent.Invoke();
            for (int i = 0; i < choicesButtons.Length; i++)
            {
                choicesButtons[i].gameObject.SetActive(false);
            }
            if (choice.nextDialId >= 0)
            {
                WriteText(choice.nextDialId);
            }
        }
    }
}
