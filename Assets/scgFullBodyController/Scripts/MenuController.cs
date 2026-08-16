using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class MenuController : MonoBehaviour
{
    // ============================================================
    // VOLUME
    // ============================================================

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    // ============================================================
    // GAMEPLAY
    // ============================================================

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;
    [SerializeField] private int defaultControllerSen = 4;

    public int mainControllerSen = 4;

    [SerializeField] private Toggle invertYToggle = null;

    // ============================================================
    // GRAPHICS
    // ============================================================

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 0.5f;

    [SerializeField] private PostProcessVolume postProcessVolume = null;

    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private Toggle fullScreenToggle = null;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    private ColorGrading colorGrading;

    // ============================================================
    // VFX
    // ============================================================

    [Header("VFX Settings")]
    [SerializeField] private TMP_Text vfxTextValue = null;
    [SerializeField] private Slider vfxSlider = null;
    [SerializeField] private float defaultVFX = 1.0f;

    public float mainVFX = 1.0f;

    // ============================================================
    // CONFIRMATION
    // ============================================================

    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPrompt = null;

    // ============================================================
    // LEVELS
    // ============================================================

    [Header("Levels To Load")]
    public string _newGameLevels;

    private string levelToLoad;

    [SerializeField] private GameObject noSavedGameDialog = null;

    // ============================================================
    // RESOLUTION
    // ============================================================

    [Header("Resolution Dropdown")]
    [SerializeField] private TMP_Dropdown resolutionDropdown = null;

    private Resolution[] resolutions;

    // ============================================================
    // START
    // ============================================================

    private void Start()
    {
        // ----------------------------
        // Resolution
        // ----------------------------

        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option =
                    resolutions[i].width +
                    " x " +
                    resolutions[i].height;

                options.Add(option);

                if (
                    resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height
                )
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);

            resolutionDropdown.value =
                currentResolutionIndex;

            resolutionDropdown.RefreshShownValue();
        }

        // ----------------------------
        // Post Processing
        // ----------------------------

        SetupColorGrading();

        // ----------------------------
        // Default values
        // ----------------------------

        if (volumeSlider != null)
        {
            volumeSlider.value =
                PlayerPrefs.GetFloat(
                    "MasterVolume",
                    defaultVolume
                );
        }

        if (ControllerSenSlider != null)
        {
            ControllerSenSlider.value =
                PlayerPrefs.GetFloat(
                    "MasterControllerSen",
                    defaultControllerSen
                );
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.value =
                PlayerPrefs.GetFloat(
                    "MasterBrightness",
                    defaultBrightness
                );
        }

        if (vfxSlider != null)
        {
            vfxSlider.value =
                PlayerPrefs.GetFloat(
                    "MasterVFX",
                    defaultVFX
                );
        }

        _qualityLevel =
            PlayerPrefs.GetInt(
                "MasterQuality",
                QualitySettings.GetQualityLevel()
            );

        _isFullScreen =
            PlayerPrefs.GetInt(
                "MasterFullScreen",
                Screen.fullScreen ? 1 : 0
            ) == 1;

        mainControllerSen =
            Mathf.RoundToInt(
                PlayerPrefs.GetFloat(
                    "MasterControllerSen",
                    defaultControllerSen
                )
            );

        mainVFX =
            PlayerPrefs.GetFloat(
                "MasterVFX",
                defaultVFX
            );

        _brightnessLevel =
            PlayerPrefs.GetFloat(
                "MasterBrightness",
                defaultBrightness
            );
    }

    // ============================================================
    // RESOLUTION
    // ============================================================

    public void SetResolution(int resolutionIndex)
    {
        if (
            resolutions == null ||
            resolutionIndex < 0 ||
            resolutionIndex >= resolutions.Length
        )
        {
            return;
        }

        Resolution resolution =
            resolutions[resolutionIndex];

        Screen.SetResolution(
            resolution.width,
            resolution.height,
            Screen.fullScreen
        );
    }

    // ============================================================
    // NEW GAME
    // ============================================================

    public void NewGameDialogYes()
    {
        if (string.IsNullOrEmpty(_newGameLevels))
        {
            Debug.LogWarning(
                "New Game Level is not assigned."
            );

            return;
        }

        SceneManager.LoadScene(
            _newGameLevels
        );
    }

    // ============================================================
    // LOAD GAME
    // ============================================================

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad =
                PlayerPrefs.GetString(
                    "SavedLevel"
                );

            SceneManager.LoadScene(
                levelToLoad
            );
        }
        else
        {
            if (noSavedGameDialog != null)
            {
                noSavedGameDialog.SetActive(true);
            }

            Debug.Log(
                "No saved game found."
            );
        }
    }

    // ============================================================
    // EXIT
    // ============================================================

    public void ExitButton()
    {
        Debug.Log(
            "Exiting game..."
        );

        Application.Quit();
    }

    // ============================================================
    // VOLUME
    // ============================================================

    public void SetVolume(float volume)
    {
        volume =
            Mathf.Clamp01(volume);

        AudioListener.volume = volume;

        if (volumeTextValue != null)
        {
            volumeTextValue.text =
                volume.ToString("0.0");
        }
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat(
            "MasterVolume",
            AudioListener.volume
        );

        PlayerPrefs.Save();

        StartCoroutine(
            ConfirmationBox()
        );

        Debug.Log(
            "Volume applied: " +
            AudioListener.volume
        );
    }

    // ============================================================
    // SENSITIVITY
    // ============================================================

    public void SetControllerSen(
        float sensitivity
    )
    {
        mainControllerSen =
            Mathf.RoundToInt(
                sensitivity
            );

        if (ControllerSenTextValue != null)
        {
            ControllerSenTextValue.text =
                sensitivity.ToString("0");
        }
    }

    public void GameplaySenApply()
    {
        if (invertYToggle != null)
        {
            PlayerPrefs.SetInt(
                "MasterInvertY",
                invertYToggle.isOn ? 1 : 0
            );
        }

        PlayerPrefs.SetFloat(
            "MasterControllerSen",
            mainControllerSen
        );

        PlayerPrefs.Save();

        StartCoroutine(
            ConfirmationBox()
        );

        Debug.Log(
            "Controller sensitivity applied: " +
            mainControllerSen
        );
    }

    // ============================================================
    // BRIGHTNESS
    // ============================================================

    private void SetupColorGrading()
    {
        if (postProcessVolume == null)
        {
            Debug.LogWarning(
                "PostProcessVolume is not assigned.",
                this
            );

            return;
        }

        if (postProcessVolume.profile == null)
        {
            Debug.LogWarning(
                "PostProcessVolume has no Profile assigned.",
                this
            );

            return;
        }

        if (
            !postProcessVolume.profile.TryGetSettings(
                out colorGrading
            )
        )
        {
            Debug.LogWarning(
                "Color Grading is not found in the Post Process Profile.",
                this
            );
        }
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel =
            Mathf.Clamp01(brightness);

        if (brightnessTextValue != null)
        {
            brightnessTextValue.text =
                _brightnessLevel.ToString("0.0");
        }

        ApplyBrightness(
            _brightnessLevel
        );
    }

    private void ApplyBrightness(
        float brightness
    )
    {
        if (colorGrading == null)
            return;

        // 0 → -2 exposure
        // 0.5 → 0 exposure
        // 1 → +2 exposure
        float exposure =
            Mathf.Lerp(
                -2f,
                2f,
                brightness
            );

        colorGrading.postExposure.value =
            exposure;
    }

    public void BrightnessApply()
    {
        PlayerPrefs.SetFloat(
            "MasterBrightness",
            _brightnessLevel
        );

        PlayerPrefs.Save();

        StartCoroutine(
            ConfirmationBox()
        );

        Debug.Log(
            "Brightness applied: " +
            _brightnessLevel
        );
    }

    // ============================================================
    // FULLSCREEN
    // ============================================================

    public void SetFullScreen(
        bool isFullScreen
    )
    {
        _isFullScreen =
            isFullScreen;
    }

    // ============================================================
    // QUALITY
    // ============================================================

    public void SetQuality(
        int qualityIndex
    )
    {
        _qualityLevel =
            Mathf.Clamp(
                qualityIndex,
                0,
                QualitySettings.names.Length - 1
            );
    }

    // ============================================================
    // GRAPHICS APPLY
    // ============================================================

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat(
            "MasterBrightness",
            _brightnessLevel
        );

        PlayerPrefs.SetInt(
            "MasterQuality",
            _qualityLevel
        );

        PlayerPrefs.SetInt(
            "MasterFullScreen",
            _isFullScreen ? 1 : 0
        );

        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(
            _qualityLevel
        );

        Screen.fullScreen =
            _isFullScreen;

        ApplyBrightness(
            _brightnessLevel
        );

        StartCoroutine(
            ConfirmationBox()
        );

        Debug.Log(
            "Graphics settings applied. " +
            "Fullscreen: " +
            _isFullScreen +
            ", Quality: " +
            _qualityLevel +
            ", Brightness: " +
            _brightnessLevel
        );
    }

    // ============================================================
    // VFX
    // ============================================================

    public void SetVFX(float vfx)
    {
        mainVFX =
            Mathf.Clamp01(vfx);

        if (vfxTextValue != null)
        {
            vfxTextValue.text =
                mainVFX.ToString("0.0");
        }
    }

    public void VFXApply()
    {
        PlayerPrefs.SetFloat(
            "MasterVFX",
            mainVFX
        );

        PlayerPrefs.Save();

        StartCoroutine(
            ConfirmationBox()
        );

        Debug.Log(
            "VFX applied: " +
            mainVFX
        );
    }

    // ============================================================
    // RESET
    // ============================================================

    public void ResetButton(
        string MenuType
    )
    {
        // ----------------------------
        // GRAPHICS
        // ----------------------------

        if (MenuType == "Graphics")
        {
            _brightnessLevel =
                defaultBrightness;

            if (brightnessSlider != null)
            {
                brightnessSlider.value =
                    defaultBrightness;
            }

            if (brightnessTextValue != null)
            {
                brightnessTextValue.text =
                    defaultBrightness.ToString("0.0");
            }

            SetBrightness(
                defaultBrightness
            );

            _qualityLevel = 1;

            if (qualityDropdown != null)
            {
                qualityDropdown.value =
                    _qualityLevel;

                qualityDropdown.RefreshShownValue();
            }

            QualitySettings.SetQualityLevel(
                _qualityLevel
            );

            _isFullScreen = false;

            if (fullScreenToggle != null)
            {
                fullScreenToggle.isOn = false;
            }

            Screen.fullScreen = false;

            // Reset resolution to current desktop resolution
            if (
                resolutions != null &&
                resolutions.Length > 0 &&
                resolutionDropdown != null
            )
            {
                Resolution currentResolution =
                    Screen.currentResolution;

                int index = 0;

                for (int i = 0; i < resolutions.Length; i++)
                {
                    if (
                        resolutions[i].width ==
                        currentResolution.width &&
                        resolutions[i].height ==
                        currentResolution.height
                    )
                    {
                        index = i;
                        break;
                    }
                }

                resolutionDropdown.value =
                    index;

                resolutionDropdown.RefreshShownValue();

                Screen.SetResolution(
                    currentResolution.width,
                    currentResolution.height,
                    false
                );
            }

            GraphicsApply();

            return;
        }

        // ----------------------------
        // AUDIO
        // ----------------------------

        if (MenuType == "Audio")
        {
            AudioListener.volume =
                defaultVolume;

            if (volumeSlider != null)
            {
                volumeSlider.value =
                    defaultVolume;
            }

            if (volumeTextValue != null)
            {
                volumeTextValue.text =
                    defaultVolume.ToString("0.0");
            }

            VolumeApply();

            return;
        }

        // ----------------------------
        // GAMEPLAY
        // ----------------------------

        if (MenuType == "Gameplay")
        {
            mainControllerSen =
                defaultControllerSen;

            if (ControllerSenTextValue != null)
            {
                ControllerSenTextValue.text =
                    defaultControllerSen.ToString("0");
            }

            if (ControllerSenSlider != null)
            {
                ControllerSenSlider.value =
                    defaultControllerSen;
            }

            if (invertYToggle != null)
            {
                invertYToggle.isOn = false;
            }

            GameplaySenApply();

            return;
        }

        // ----------------------------
        // VFX
        // ----------------------------

        if (MenuType == "VFX")
        {
            mainVFX =
                defaultVFX;

            if (vfxSlider != null)
            {
                vfxSlider.value =
                    defaultVFX;
            }

            if (vfxTextValue != null)
            {
                vfxTextValue.text =
                    defaultVFX.ToString("0.0");
            }

            VFXApply();

            return;
        }

        Debug.LogWarning(
            "Unknown reset menu type: " +
            MenuType
        );
    }

    // ============================================================
    // CONFIRMATION BOX
    // ============================================================

    public IEnumerator ConfirmationBox()
    {
        if (comfirmationPrompt == null)
            yield break;

        comfirmationPrompt.SetActive(
            true
        );

        yield return new WaitForSeconds(
            2f
        );

        comfirmationPrompt.SetActive(
            false
        );
    }
}