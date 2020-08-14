using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationUIManager : SingletonBehaviour<ExplorationUIManager>
{
    public GameObject panelExploration;

    // for prototype
    public Image panelError;
    public void ButonComeBackHomeClicked()
    {
        panelError.gameObject.SetActive(false);
        panelExploration.SetActive(false);
        HomeUIManager.Inst.panelHome.SetActive(true);
        GameManager.Inst.ReturnHome();
    }

    public void NoticeUnderDevelopment()
    {
        panelError.gameObject.SetActive(true);
    }

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
}
