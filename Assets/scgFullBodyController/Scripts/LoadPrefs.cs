using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("Quality Level Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;

    [Header("Full Screen Settings")]
    [SerializeField] private Toggle fullScreenToggle = null;

    [Header("Sensitivity Settings")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;

    [Header("Invert Y Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if(PlayerPrefs.HasKey("MasterVolume"))
            {
                float localValue = PlayerPrefs.GetFloat("MasterVolume");

                volumeTextValue.text = localValue.ToString("0.0");
                volumeSlider.value = localValue;
                AudioListener.volume = localValue;


            }
            else
            {
                menuController.ResetButton("Audio");

            }
            if(PlayerPrefs.HasKey("MasterQuality"))
            {
                int localValue = PlayerPrefs.GetInt("MasterQuality");

                qualityDropdown.value = localValue;
                QualitySettings.SetQualityLevel(localValue);
            }
            if(PlayerPrefs.HasKey("MasterFullScreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("MasterFullScreen");

                if(localFullscreen == 1)
                {
                    fullScreenToggle.isOn = true;
                    Screen.fullScreen = true;
                }
                else
                {
                    fullScreenToggle.isOn = false;
                    Screen.fullScreen = false;
                }
            }
            if(PlayerPrefs.HasKey("MasterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("MasterBrightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;
            }
            if(PlayerPrefs.HasKey("MasterSensitivity"))
            {
                float localSen = PlayerPrefs.GetFloat("MasterSensitivity");

                ControllerSenTextValue.text = localSen.ToString("0");
                ControllerSenSlider.value = localSen;
                menuController.mainControllerSen = Mathf.RoundToInt(localSen);
            }
            if(PlayerPrefs.HasKey("MasterInvertY"))
            {
                if(PlayerPrefs.GetInt("MasterInvertY") == 1)
                {
                    invertYToggle.isOn = true;
                }
                else
                {
                    invertYToggle.isOn = false;
                }
            }
        }
    }
}
