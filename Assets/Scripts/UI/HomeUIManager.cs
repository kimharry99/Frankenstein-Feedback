using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : SingletonBehaviour<HomeUIManager>
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

    // for debugging
    public void ClickedSendTime()
    {
        GameManager.Inst.SendTime(1);
    }
    #region HomePanel methods
    public void ButtonSettingClicked()
    {
        PanelHome.SetActive(false);
        PanelSetting.SetActive(true);
    }

    public void ButtonExploreClicked()
    {
        SceneManager.LoadScene(1);

    }

    public void ButtonCraftClicked()
    {
        PanelHome.SetActive(false);
        PanelCrafting.SetActive(true);
        Debug.Log("1111");
    }

    public void ButtonStorageClicked()
    {
        PanelHome.SetActive(false);
        PanelStorage.SetActive(true);
    }
    #endregion


    #region CraftingPanel methods
    public void ButtonItemClicked()
    {
        print("ButtonItemClicked");
    }

    public void ButtonCreateClicked()
    {
        print("ButtonCreateClicked");
    }
    #endregion
}
