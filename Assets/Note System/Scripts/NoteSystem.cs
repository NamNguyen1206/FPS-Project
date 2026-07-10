using UnityEngine;
using TMPro;

public class NoteSystem : MonoBehaviour
{
    [Header("Note Content")]
    [TextArea(5, 10)]
    public string noteContent;

    [Header("UI")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private GameObject interactionText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    [Header("Settings")]
    [SerializeField] private bool destroyAfterReading = true;

    private bool isReading = false;

    public void OpenNote()
    {
        if (isReading)
            return;

        isReading = true;

        notePanel.SetActive(true);
        noteText.text = noteContent;

        if (interactionText != null)
            interactionText.SetActive(false);

        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);

        // Pause game
        Time.timeScale = 0f;
        PauseMenu.isPaused = true;

        // Show Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        if (!isReading)
            return;

        isReading = false;

        notePanel.SetActive(false);

        if (audioSource != null && closeSound != null)
            audioSource.PlayOneShot(closeSound);

        // Resume game
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;

        // Hide Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (destroyAfterReading)
        {
            Destroy(gameObject);
        }
    }
}