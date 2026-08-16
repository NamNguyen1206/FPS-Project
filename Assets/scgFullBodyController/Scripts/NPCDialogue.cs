// using UnityEngine;
// using TMPro;

// public class NPCDialogue : MonoBehaviour
// {
//     [Header("Dialogue")]
//     [TextArea(3,6)]
//     public string[] dialogue;

//     public string npcName = "NPC";

//     [Header("UI")]
//     public GameObject dialoguePanel;
//     public TextMeshProUGUI npcNameText;
//     public TextMeshProUGUI dialogueText;

//     private bool isTalking = false;
//     private int currentDialogue = 0;

//     public void NPCInteract()
//     {
//         if (!isTalking)
//         {
//             StartDialogue();
//         }
//         else
//         {
//             NextDialogue();
//         }
//     }
//         void StartDialogue()
//     {
//         isTalking = true;

//         currentDialogue = 0;

//         dialoguePanel.SetActive(true);

//         npcNameText.text = npcName;

//         dialogueText.text = dialogue[currentDialogue];
//     }

//     void NextDialogue()
//     {
//         currentDialogue++;

//         if (currentDialogue >= dialogue.Length)
//         {
//             EndDialogue();
//             return;
//         }

//         dialogueText.text = dialogue[currentDialogue];
//     }

//     void EndDialogue()
//     {
//         dialoguePanel.SetActive(false);

//         isTalking = false;

//         currentDialogue = 0;
//     }
// }
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    // ============================================================
    // DIALOGUE
    // ============================================================

    [Header("Dialogue")]

    [TextArea(3, 6)]
    public string[] dialogue;

    public string npcName = "NPC";


    // ============================================================
    // UI
    // ============================================================

    [Header("UI")]

    public GameObject dialoguePanel;

    public TextMeshProUGUI npcNameText;

    public TextMeshProUGUI dialogueText;


    // ============================================================
    // STATE
    // ============================================================

    private bool isTalking = false;

    private int currentDialogue = 0;


    // Cho DoorRayCast biết NPC đang dialogue hay không
    public bool IsTalking => isTalking;


    // ============================================================
    // NPC INTERACTION
    // ============================================================

    public void NPCInteract()
    {
        // Nếu chưa nói chuyện
        if (!isTalking)
        {
            StartDialogue();
        }
        else
        {
            // Nếu đang nói chuyện
            NextDialogue();
        }
    }


    // ============================================================
    // START DIALOGUE
    // ============================================================

    private void StartDialogue()
    {
        // Kiểm tra dialogue có tồn tại không
        if (dialogue == null || dialogue.Length == 0)
        {
            Debug.LogWarning(
                $"NPC [{npcName}] has no dialogue."
            );

            return;
        }

        isTalking = true;

        currentDialogue = 0;


        // Hiện Dialogue Panel
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }


        // Hiện tên NPC
        if (npcNameText != null)
        {
            npcNameText.text = npcName;
        }


        // Hiện câu thoại đầu tiên
        if (dialogueText != null)
        {
            dialogueText.text = dialogue[currentDialogue];
        }
    }


    // ============================================================
    // NEXT DIALOGUE
    // ============================================================

    private void NextDialogue()
    {
        currentDialogue++;

        // Đã hết dialogue
        if (currentDialogue >= dialogue.Length)
        {
            EndDialogue();

            return;
        }


        // Hiển thị câu tiếp theo
        if (dialogueText != null)
        {
            dialogueText.text = dialogue[currentDialogue];
        }
    }


    // ============================================================
    // END DIALOGUE
    // ============================================================

    private void EndDialogue()
    {
        // Ẩn Dialogue Panel
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }


        // Đặt trạng thái về không nói chuyện
        isTalking = false;

        currentDialogue = 0;
    }
}