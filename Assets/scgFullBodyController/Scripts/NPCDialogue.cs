using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(3,6)]
    public string[] dialogue;

    public string npcName = "NPC";

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;

    private bool isTalking = false;
    private int currentDialogue = 0;

    public void NPCInteract()
    {
        if (!isTalking)
        {
            StartDialogue();
        }
        else
        {
            NextDialogue();
        }
    }
        void StartDialogue()
    {
        isTalking = true;

        currentDialogue = 0;

        dialoguePanel.SetActive(true);

        npcNameText.text = npcName;

        dialogueText.text = dialogue[currentDialogue];
    }

    void NextDialogue()
    {
        currentDialogue++;

        if (currentDialogue >= dialogue.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogue[currentDialogue];
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        isTalking = false;

        currentDialogue = 0;
    }
}
