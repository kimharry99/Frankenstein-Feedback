using System.Collections;
using System.Collections.Generic;
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

    #region HomePanel methods
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
        Debug.Log("1111");
    }

    public void ButtonStorageClicked()
    {
        PanelHome.SetActive(false);
        PanelStorage.SetActive(true);
    }
    #endregion

    #region ExplorationPanel methods
    public void ButtonOption1Clicked()
    {
        Debug.Log("버튼 1을 클릭했습니다.");
    }
    public void ButtonOption2Clicked()
    {
        Debug.Log("버튼 2를 클릭했습니다.");
    }
    public void ButtonOption3Clicked()
    {
        Debug.Log("버튼 3을 클릭했습니다.");
    }
    public void ButtonOption4Clicked()
    {
        Debug.Log("버튼 4를 클릭했습니다.");
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
