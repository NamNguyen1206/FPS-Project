using UnityEngine;

namespace NavKeypad
{
    public class KeypadInteractionZone : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private float interactDistance = 3f;

        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera keypadCamera;

        [SerializeField] private MonoBehaviour playerController;
        [SerializeField] private MonoBehaviour mouseLook;

        [SerializeField] private KeypadInteractionFPV keypadInput;

        [Header("UI")]
        [SerializeField] private GameObject interactionText;

        private bool isUsingKeypad = false;

        private void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            if (keypadCamera != null)
                keypadCamera.enabled = false;

            if (keypadInput != null)
                keypadInput.enabled = false;

            if (interactionText != null)
                interactionText.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (!isUsingKeypad)
            {
                bool canInteract = CanInteract();

                if (interactionText != null)
                    interactionText.SetActive(canInteract);

                if (canInteract && Input.GetKeyDown(KeyCode.E))
                {
                    EnterKeypadMode();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ExitKeypadMode();
                }
            }
        }

        bool CanInteract()
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                return hit.collider.transform.IsChildOf(transform);
            }

            return false;
        }

        public void EnterKeypadMode()
        {
            isUsingKeypad = true;

            if (interactionText != null)
                interactionText.SetActive(false);

            if (playerController != null)
                playerController.enabled = false;

            if (mouseLook != null)
                mouseLook.enabled = false;

            if (mainCamera != null)
                mainCamera.enabled = false;

            if (keypadCamera != null)
                keypadCamera.enabled = true;

            if (keypadInput != null)
                keypadInput.enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ExitKeypadMode()
        {
            isUsingKeypad = false;

            if (mainCamera != null)
                mainCamera.enabled = true;

            if (keypadCamera != null)
                keypadCamera.enabled = false;

            if (playerController != null)
                playerController.enabled = true;

            if (mouseLook != null)
                mouseLook.enabled = true;

            if (keypadInput != null)
                keypadInput.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
