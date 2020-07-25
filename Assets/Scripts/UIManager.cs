using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    [Header("Time UI")]
    public GameObject TextDay;
    public GameObject TextTime;

    [Header("Energy UI")]
    public GameObject SliderEnergy;

    [Header("Sub Panels")]
    public GameObject PanelSetting;
    public GameObject PanelHome;
    public GameObject PanelExploration;
    public GameObject PanelCrafting;
    public GameObject PanelStorage;

    public void ButtonSettingClicked()
    {
        PanelHome.SetActive(false);
        PanelSetting.SetActive(true);
    }

    public void ButtonExploreClicked()
    {
        PanelExploration.SetActive(true);
        PanelHome.SetActive(false);
    }

    public void ButtonCraftClicked()
    {
        PanelHome.SetActive(false);
        PanelCrafting.SetActive(true);
    }

    public void ButtonStorageClicked()
    {
        PanelHome.SetActive(false);
        PanelStorage.SetActive(true);
    }

}
