using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteSystem : MonoBehaviour
{
    [Header("Note Content")]
    [TextArea(5, 10)]
    public string[] notePages;

    [Header("UI")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private Text noteText;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    [Header("Player")]
    [SerializeField] private GameObject playerModel;

    //added Part 2
    public AudioClip nextPageSounds;
    private int currentPage = 0;

    [Header("Settings")]
    [SerializeField] private bool destroyAfterReading = true;

    private bool isReading = false;

    public void OpenNote()
    {
        if (isReading)
            return;

        isReading = true;

        notePanel.SetActive(true);
        currentPage = 0;
        ShowCurrentPage();

        if (interactionText != null)
            interactionText.SetActive(false);

        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);

        // Ẩn model player
        if (playerModel != null)
            playerModel.SetActive(false);

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
    
        // Hiện lại model player
        if (playerModel != null)
            playerModel.SetActive(true);

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

    private void ShowCurrentPage()
    {
        if (notePages.Length == 0)
            return;

        noteText.text = notePages[currentPage];

        previousButton.SetActive(currentPage > 0);
        nextButton.SetActive(currentPage < notePages.Length - 1);
    }
    
    public void NextPage()
    {
        if (currentPage < notePages.Length - 1)
        {
            currentPage++;
            ShowCurrentPage();

            if (audioSource != null && nextPageSounds != null)
                audioSource.PlayOneShot(nextPageSounds);
        }
    }
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowCurrentPage();

            if (audioSource != null && nextPageSounds != null)
                audioSource.PlayOneShot(nextPageSounds);
        }
    }
}