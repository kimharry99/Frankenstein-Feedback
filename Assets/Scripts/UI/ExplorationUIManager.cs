using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationUIManager : SingletonBehaviour<ExplorationUIManager>
{
    [Serializable]
    private struct ContentSlot
    {
        public Text eventText;
        public Image eventImage;
    }

    public GameObject panelExploration;
    private List<ContentSlot> eventContents = new List<ContentSlot>();
    [SerializeField]
    private Button[] buttonOptions = new Button[4];

    protected override void Awake()
    {
        base.Awake();
        for(int i=0;i<5;i++)
        {
            ContentSlot contentSlot;
            contentSlot.eventText = panelExploration.transform.GetChild(0).GetChild(i).GetComponent<Text>();
            contentSlot.eventImage = panelExploration.transform.GetChild(0).GetChild(5 + i).GetComponent<Image>();
            eventContents.Add(contentSlot);
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
        if(eventContents[0].eventText != null)
        {
            eventContents[0].eventText.text = titleText;
        }
        else
        {
            Debug.LogError("eventTexts[0] is null");
        }
    }

    /// <summary>
    /// UI에 표시된 이벤트 텍스트와 이미지를 한 칸 위로 올린다.
    /// </summary>
    private void ShiftEventContents()
    {
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
