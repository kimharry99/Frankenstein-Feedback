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
    public GameObject PanelDisassemble;
    public GameObject PanelCrafting;
    public GameObject PanelAssemble;
    public GameObject PanelStorage;

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

    public void ButtonDisassembleClicked()
    {
        PanelHome.SetActive(false);
        PanelDisassemble.SetActive(true);
    }

    public void ButtonCraftClicked()
    {
        PanelHome.SetActive(false);
        PanelCrafting.SetActive(true);
        Debug.Log("1111");
    }

    public void ButtonAssembleClicked()
    {
        PanelHome.SetActive(false);
        PanelAssemble.SetActive(true);
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

    #region AssemblePanel methods
    public void ButtonDeadBodyClicked()
    {
        print("ButtonDeadBodyClicked");
    }

    public void ButtonEquipClicked()
    {
        print("ButtonEquipClicked");
    }
    #endregion
}
