using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;
    [SerializeField] private int defaultControllerSen = 4;
    public int mainControllerSen = 4;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1.0f;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown ;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPrompt = null;

    [Header("Levels To Load ")]
    public string _newGameLevels;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution GropDowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevels);
    }
    public void LoadGameDialogYes()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
            Debug.Log("No saved game found.");
        }
    }
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Exiting game...");
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
        Debug.Log("Volume applied: " + AudioListener.volume);
    }
    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        ControllerSenTextValue.text = sensitivity.ToString("0");
    }
    public void GameplaySenApply()
    {
        if(invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("MasterInvertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MasterInvertY", 0);
        }
        PlayerPrefs.SetFloat("MasterControllerSen", mainControllerSen);
        StartCoroutine(ConfirmationBox());
        Debug.Log("Controller sensitivity applied: " + mainControllerSen);
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
        // Here you would apply the brightness setting to your game's graphics settings
    }
    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("MasterBrightness", _brightnessLevel);
        PlayerPrefs.SetInt("MasterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("MasterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
        StartCoroutine(ConfirmationBox());
        Debug.Log("Graphics settings applied. Fullscreen: " + _isFullScreen + ", Quality Level: " + _qualityLevel);
    }
    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1; // Assuming 1 is the default quality level
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if(MenuType == "Gameplay")
        {
            ControllerSenTextValue.text = defaultControllerSen.ToString("0");
            ControllerSenSlider.value = defaultControllerSen;
            mainControllerSen = defaultControllerSen;
            invertYToggle.isOn = false;
            GameplaySenApply();
        }
        Debug.Log("Settings reset to default.");
    }
    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2f);
        comfirmationPrompt.SetActive(false);
    }

}
