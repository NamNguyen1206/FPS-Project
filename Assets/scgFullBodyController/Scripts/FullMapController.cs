using UnityEngine;
using scgFullBodyController;

public class FullMapController : MonoBehaviour
{
    [Header("UI")]
    public GameObject fullMapPanel;

    [Header("Mini Map")]
    public GameObject miniMap;

    private bool mapOpened = false;
    public CameraController cameraController;

    void Start()
    {
        fullMapPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapOpened = !mapOpened;

            fullMapPanel.SetActive(mapOpened);

            if (miniMap != null)
                miniMap.SetActive(!mapOpened);

            //cameraController.enabled = !mapOpened;
            Time.timeScale = mapOpened ? 0 : 1;

            Cursor.visible = mapOpened;
            Cursor.lockState = mapOpened ?
                CursorLockMode.None :
                CursorLockMode.Locked;

            Debug.Log("Time Scale = " + Time.timeScale);
        }
        //if (Input.GetKeyDown(KeyCode.M))
        // {
        //     mapOpened = !mapOpened;

        //     fullMapPanel.SetActive(mapOpened);

        //     if (mapOpened)
        //     {
        //         cameraController.enabled = false;

        //         Cursor.lockState = CursorLockMode.None;
        //         Cursor.visible = true;

        //         Time.timeScale = 0f;
        //     }
        //     else
        //     {
        //         Time.timeScale = 1f;

        //         Cursor.lockState = CursorLockMode.Locked;
        //         Cursor.visible = false;

        //         cameraController.enabled = true;
        //     }
        // }
    }
}