using UnityEngine;

public class NoteRaycast : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject notePanel;

    private NoteSystem currentNote;

    void Update()
    {
        DetectNote();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentNote != null)
            {
                currentNote.OpenNote();
            }
        }
    }

    void DetectNote()
    {
        // Nếu NotePanel đang mở thì không cho hiện ActionText
        if (notePanel.activeSelf)
        {
            interactionText.SetActive(false);
            return;
        }
        currentNote = null;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            NoteSystem note = hit.collider.GetComponent<NoteSystem>();

            if (note != null)
            {
                currentNote = note;

                if (interactionText != null)
                    interactionText.SetActive(true);

                return;
            }
        }

        if (interactionText != null)
            interactionText.SetActive(false);
    }
}
