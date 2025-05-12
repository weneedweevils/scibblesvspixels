using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayOptions : MonoBehaviour
{
    public enum Options { None, VSync, Fullscreen, Resolution };
    public Options option;
    private Toggle toggle;
    private TMPro.TMP_Dropdown resolutionDropdown;
    private List<ResItem> resItems;

    void Awake()
    {
        if (option == Options.Resolution)
        {
            resolutionDropdown = GetComponent<TMPro.TMP_Dropdown>();
            resItems = new List<ResItem>();
            if (resolutionDropdown == null)
            {
                Debug.LogErrorFormat("Resolution Display Option {0} is missing TMPro Dropdown component", gameObject.name);
                return;
            }
            PopulateResolutions();
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }
        else
        {
            toggle = GetComponent<Toggle>();
            if (toggle == null)
            {
                Debug.LogErrorFormat("Toggle Display Option {0} is missing Toggle component", gameObject.name);
                return;
            }
            LoadSetting();
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    private void LoadSetting()
    {
        switch (option)
        {
            case Options.VSync:
                toggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
                QualitySettings.vSyncCount = toggle.isOn ? 1 : 0;
                break;
            case Options.Fullscreen:
                toggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
                Screen.fullScreen = toggle.isOn;
                break;
            case Options.Resolution:
                // Resolution is handled separately in PopulateResolutions()
                break;
        }
    }

    public void OnToggleChanged(bool value)
    {
        switch (option)
        {
            case Options.VSync:
                QualitySettings.vSyncCount = value ? 1 : 0;
                PlayerPrefs.SetInt("VSync", value ? 1 : 0);
                break;
            case Options.Fullscreen:
                Screen.fullScreen = value;
                PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
                break;
        }
        PlayerPrefs.Save();
    }

    private void PopulateResolutions()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        Resolution[] resolutions = Screen.resolutions;

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            ResItem option = new ResItem(resolutions[i]);
            if (FilterOut(option))
                continue;

            bool add = true;
            foreach(ResItem other in resItems)
            {
                if (option.Equals(other))
                {
                    add = false;
                    break;
                }
            }
            if (add)
            {
                resItems.Add(option);
                options.Add(option.GetString());
                if (option.width == Screen.currentResolution.width &&
                    option.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = resItems.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    private void SetResolution(int index)
    {
        if (!Screen.fullScreen) // Only allow resolution change if not in fullscreen
        {
            if (index >= 0 && index < resItems.Count)
            {
                resItems[index].SetResolution();
                PlayerPrefs.SetInt("ResolutionIndex", index);
                PlayerPrefs.Save();
            }
        }
    }

    private bool FilterOut(ResItem item)
    {
        return false;
    }
}

public class ResItem
{
    public int width { get; private set; }
    public int height { get; private set; }
    public ResItem(Resolution resolution)
    {
        width = resolution.width;
        height = resolution.height;
    }
    public ResItem(int horizontal, int vertical)
    {
        this.width = horizontal;
        this.height = vertical;
    }
    public bool Equals(ResItem other)
    {
        return (width == other.width && height == other.height);
    }
    public void SetResolution()
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
    public string GetString()
    {
        return string.Format("{0}x{1}", width, height);
    }
}