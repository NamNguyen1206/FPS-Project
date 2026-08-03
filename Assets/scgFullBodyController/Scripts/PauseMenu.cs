using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    public GameObject playerModel;
    public static bool isPaused;

    void Start()
    {
        if (pauseMenuUI != null)
        pauseMenuUI.SetActive(false);
        Resume();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    private void Pause()
    {
        if (pauseMenuUI != null)
        pauseMenuUI.SetActive(true);  
        if (playerModel != null)
        playerModel.SetActive(false);      
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (playerModel != null)
            playerModel.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }
}
