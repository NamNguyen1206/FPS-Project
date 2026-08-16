using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    // ============================================================
    // VOLUME
    // ============================================================

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    // ============================================================
    // BRIGHTNESS
    // ============================================================

    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    // ============================================================
    // VFX
    // ============================================================

    [Header("VFX Settings")]
    [SerializeField] private Slider vfxSlider = null;
    [SerializeField] private TMP_Text vfxTextValue = null;

    // ============================================================
    // QUALITY
    // ============================================================

    [Header("Quality Level Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;

    // ============================================================
    // FULLSCREEN
    // ============================================================

    [Header("Full Screen Settings")]
    [SerializeField] private Toggle fullScreenToggle = null;

    // ============================================================
    // SENSITIVITY
    // ============================================================

    [Header("Sensitivity Settings")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;

    // ============================================================
    // INVERT Y
    // ============================================================

    [Header("Invert Y Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        if (!canUse)
            return;

        if (menuController == null)
        {
            Debug.LogWarning(
                "LoadPrefs: MenuController is not assigned.",
                this
            );
        }

        // ========================================================
        // VOLUME
        // ========================================================

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float localValue =
                PlayerPrefs.GetFloat(
                    "MasterVolume"
                );

            if (volumeTextValue != null)
            {
                volumeTextValue.text =
                    localValue.ToString("0.0");
            }

            if (volumeSlider != null)
            {
                volumeSlider.value =
                    localValue;
            }

            AudioListener.volume =
                localValue;
        }
        else
        {
            if (menuController != null)
            {
                menuController.ResetButton(
                    "Audio"
                );
            }
        }

        // ========================================================
        // QUALITY
        // ========================================================

        if (PlayerPrefs.HasKey("MasterQuality"))
        {
            int localValue =
                PlayerPrefs.GetInt(
                    "MasterQuality"
                );

            localValue =
                Mathf.Clamp(
                    localValue,
                    0,
                    QualitySettings.names.Length - 1
                );

            if (qualityDropdown != null)
            {
                qualityDropdown.value =
                    localValue;

                qualityDropdown.RefreshShownValue();
            }

            QualitySettings.SetQualityLevel(
                localValue
            );
        }

        // ========================================================
        // FULLSCREEN
        // ========================================================

        if (PlayerPrefs.HasKey("MasterFullScreen"))
        {
            int localFullscreen =
                PlayerPrefs.GetInt(
                    "MasterFullScreen"
                );

            bool fullscreen =
                localFullscreen == 1;

            if (fullScreenToggle != null)
            {
                fullScreenToggle.isOn =
                    fullscreen;
            }

            Screen.fullScreen =
                fullscreen;
        }

        // ========================================================
        // BRIGHTNESS
        // ========================================================

        if (PlayerPrefs.HasKey("MasterBrightness"))
        {
            float localBrightness =
                PlayerPrefs.GetFloat(
                    "MasterBrightness"
                );

            localBrightness =
                Mathf.Clamp01(
                    localBrightness
                );

            if (brightnessSlider != null)
            {
                brightnessSlider.value =
                    localBrightness;
            }

            if (menuController != null)
            {
                menuController.SetBrightness(
                    localBrightness
                );
            }
            else if (brightnessTextValue != null)
            {
                brightnessTextValue.text =
                    localBrightness.ToString("0.0");
            }
        }
        else
        {
            if (menuController != null)
            {
                menuController.ResetButton(
                    "Graphics"
                );
            }
        }

        // ========================================================
        // VFX
        // ========================================================

        if (PlayerPrefs.HasKey("MasterVFX"))
        {
            float localVFX =
                PlayerPrefs.GetFloat(
                    "MasterVFX"
                );

            localVFX =
                Mathf.Clamp01(
                    localVFX
                );

            if (vfxSlider != null)
            {
                vfxSlider.value =
                    localVFX;
            }

            if (menuController != null)
            {
                menuController.SetVFX(
                    localVFX
                );
            }
            else if (vfxTextValue != null)
            {
                vfxTextValue.text =
                    localVFX.ToString("0.0");
            }
        }
        else
        {
            if (menuController != null)
            {
                menuController.ResetButton(
                    "VFX"
                );
            }
        }

        // ========================================================
        // CONTROLLER SENSITIVITY
        // ========================================================

        // Dùng cùng key với MenuController:
        // MasterControllerSen

        if (PlayerPrefs.HasKey("MasterControllerSen"))
        {
            float localSen =
                PlayerPrefs.GetFloat(
                    "MasterControllerSen"
                );

            if (ControllerSenTextValue != null)
            {
                ControllerSenTextValue.text =
                    localSen.ToString("0");
            }

            if (ControllerSenSlider != null)
            {
                ControllerSenSlider.value =
                    localSen;
            }

            if (menuController != null)
            {
                menuController.mainControllerSen =
                    Mathf.RoundToInt(
                        localSen
                    );
            }
        }
        else
        {
            if (menuController != null)
            {
                menuController.ResetButton(
                    "Gameplay"
                );
            }
        }

        // ========================================================
        // INVERT Y
        // ========================================================

        if (PlayerPrefs.HasKey("MasterInvertY"))
        {
            bool inverted =
                PlayerPrefs.GetInt(
                    "MasterInvertY"
                ) == 1;

            if (invertYToggle != null)
            {
                invertYToggle.isOn =
                    inverted;
            }
        }
    }
}