using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationUIManager : SingletonBehaviour<ExplorationUIManager>
{
    public GameObject panelExploration;
    private List<Text> eventTexts = new List<Text>();
    private List<Image> eventImages = new List<Image>();

    protected override void Awake()
    {
        base.Awake();
        for(int i=0;i<5;i++)
        {
            eventTexts.Add(panelExploration.transform.GetChild(0).GetChild(i).GetComponent<Text>());
            eventImages.Add(panelExploration.transform.GetChild(0).GetChild(5 + i).GetComponent<Image>());
    }
    }

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
    /// <summary>
    /// UI에 @event에 관련한 텍스트, 이미지를 출력한다.
    /// </summary>
    public void NoticeEvent(ExplorationEvent @event)
    {
        NoticeEventTitle(@event.titleText);
    }

    /// <summary>
    /// EventTitle을 UI에 출력한다.
    /// </summary>
    private void NoticeEventTitle(string titleText)
    {
        if(eventTexts[0] != null)
        {
            eventTexts[0].text = titleText;
        }
        else
        {
            Debug.LogError("eventTexts[0] is null");
        }
    }

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
