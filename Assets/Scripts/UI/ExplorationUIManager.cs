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
        NoticeEventText(@event.titleText);
        if(@event.type == ExplorationEvent.EventType.ItemDiscovery)
        {
            ItemDiscoveryEvent itemDiscoveryEvent = (ItemDiscoveryEvent)@event;
            NoticeEventItemImage(itemDiscoveryEvent.foundedItem.itemImage);
        }
        NoticeEventText(@event.content);
        NoticeOptions(@event.optionTexts);
    }

    /// <summary>
    /// eventText를 UI의 마지막 줄에 출력한다.
    /// </summary>
    private void NoticeEventText(string eventText)
    {
        ShiftEventContents();
        if(eventContents[eventContents.Count - 1].eventText != null)
        {
            eventContents[eventContents.Count - 1].eventText.text = eventText;
        }
        else
        {
            Debug.LogError("eventContents[" + (eventContents.Count - 1) + "].eventText is null");
        }
    }

    /// <summary>
    /// 아이템 발견 이벤트의 발견한 아이템 이미지를 마지막 줄에 출력한다. 
    /// </summary>
    /// <param name="itemImage"></param>
    private void NoticeEventItemImage(Sprite itemImage)
    {
        ShiftEventContents();
        if(eventContents[eventContents.Count - 1].eventImage != null)
        {
            eventContents[eventContents.Count - 1].eventImage.gameObject.SetActive(true);
            eventContents[eventContents.Count - 1].eventImage.sprite = itemImage;
        }
        else
        {
            Debug.LogError("eventContents[" + (eventContents.Count - 1) + "].eventImage is null");
        }
    }

    /// <summary>
    /// event의 optionTexts를 option 버튼에 표시한다.
    /// </summary>
    /// <param name="optionTexts"></param>
    private void NoticeOptions(List<string> optionTexts)
    {
        for(int i=0;i<optionTexts.Count;i++)
        {
            buttonOptions[i].interactable = true;
            buttonOptions[i].transform.GetChild(0).GetComponent<Text>().text = optionTexts[i];
        }
        for(int i=optionTexts.Count;i<4;i++)
        {
            buttonOptions[i].interactable = false;
            buttonOptions[i].transform.GetChild(0).GetComponent<Text>().text = null;
        }
    }

    /// <summary>
    /// UI에 표시된 이벤트 텍스트와 이미지를 한 칸 위로 올린다.
    /// </summary>
    /// for debugging public을 private으로 변경
    public void ShiftEventContents()
    {
        for(int i=0; i<eventContents.Count - 1;i++)
        {
            eventContents[i].eventText.text = eventContents[i + 1].eventText.text;
            if(eventContents[i + 1].eventImage.IsActive())
            {
                eventContents[i].eventImage.gameObject.SetActive(true);
                eventContents[i].eventImage.sprite = eventContents[i + 1].eventImage.sprite;
                eventContents[i + 1].eventImage.gameObject.SetActive(false);
            }
            else
            {
                eventContents[i].eventImage.gameObject.SetActive(false);
            }
        }
        eventContents[eventContents.Count - 1].eventImage.sprite = null;
        eventContents[eventContents.Count - 1].eventImage.gameObject.SetActive(false);
        eventContents[eventContents.Count - 1].eventText.text = null;
    }   
    
    /// <summary>
    /// @event의 option을 선택했을 때 발생하는 상황을 Button에 등록한다.
    /// </summary>
    /// <param name="event"></param>
    public void AddEventsToButton(ExplorationEvent @event)
    {
        if(@event.OptionNumber >= 0)
        {
            buttonOptions[0].onClick.AddListener(@event.Option0);
            if(@event.OptionNumber >= 1)
            {
                buttonOptions[1].onClick.AddListener(@event.Option1);
                if (@event.OptionNumber >= 2)
                {
                    buttonOptions[2].onClick.AddListener(@event.Option2);
                    if (@event.OptionNumber >= 3)
                    {
                        buttonOptions[3].onClick.AddListener(@event.Option3);
                    }
                }
            }
        }
    }

    /// <summary>
    /// option 버튼에 등록되어 있는 모든 이벤트를 제거한다.
    /// </summary>
    public void RemoveEventsFromButton()
    {
        for(int i=0;i<buttonOptions.Length;i++)
        {
            buttonOptions[i].onClick.RemoveAllListeners();
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
