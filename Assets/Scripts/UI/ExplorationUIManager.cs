using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationUIManager : SingletonBehaviour<ExplorationUIManager>
{
    [System.Serializable]
    private struct ContentSlot
    {
        public Text eventText;
        public Image eventImage;
    }

    public GameObject panelExploration;
    public GameObject panelCollapseWarning;
    private List<ContentSlot> contentsEvent = new List<ContentSlot>();
    [SerializeField]
    private Button[] buttonOptions = new Button[4];
    private Button[] buttonWarning = new Button[2];
    private Button ButtonContinue { get { return buttonWarning[0]; } }
    private Button ButtonCancel { get { return buttonWarning[1]; } }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            ContentSlot contentSlot;
            contentSlot.eventText = panelExploration.transform.GetChild(0).GetChild(i).GetComponent<Text>();
            contentSlot.eventImage = panelExploration.transform.GetChild(0).GetChild(5 + i).GetComponent<Image>();
            contentsEvent.Add(contentSlot);
        }
        buttonWarning[0] = panelCollapseWarning.transform.GetChild(0).GetComponent<Button>();
        buttonWarning[1] = panelCollapseWarning.transform.GetChild(1).GetComponent<Button>();
    }

    protected override void Awake()
    {
        base.Awake();
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

    #region Notice Event Methods
    /// <summary>
    /// UI에 @event에 관련한 텍스트, 이미지를 출력한다.
    /// </summary>
    public void NoticeEvent(ExplorationEvent @event)
    {
        Debug.Log("발생한 이벤트 : " + @event.titleText);
        NoticeEventText(@event.titleText);
        if(@event.phase == ExplorationEvent.EventPhase.SearchingItem)
        {
            ItemDiscoveryEvent itemDiscoveryEvent = (ItemDiscoveryEvent)@event;
            if(itemDiscoveryEvent.foundItem != null)
                NoticeEventItemImage(itemDiscoveryEvent.foundItem.itemImage);
        }
        NoticeEventText(@event.content);
        NoticeOptions(@event);
    }

    /// <summary>
    /// eventText를 UI의 마지막 줄에 출력한다.
    /// </summary>
    private void NoticeEventText(string eventText)
    {
        if(eventText != "")
        {
            ShiftEventContents();
            if (contentsEvent[contentsEvent.Count - 1].eventText != null)
            {
                contentsEvent[contentsEvent.Count - 1].eventText.text = eventText;
            }
            else
            {
                Debug.LogError("eventContents[" + (contentsEvent.Count - 1) + "].eventText is null");
            }
        }
    }

    /// <summary>
    /// 아이템 발견 이벤트의 발견한 아이템 이미지를 마지막 줄에 출력한다. 
    /// </summary>
    /// <param name="itemImage"></param>
    private void NoticeEventItemImage(Sprite itemImage)
    {
        ShiftEventContents();
        if(contentsEvent[contentsEvent.Count - 1].eventImage != null)
        {
            contentsEvent[contentsEvent.Count - 1].eventImage.gameObject.SetActive(true);
            contentsEvent[contentsEvent.Count - 1].eventImage.sprite = itemImage;
        }
        else
        {
            Debug.LogError("eventContents[" + (contentsEvent.Count - 1) + "].eventImage is null");
        }
    }

    /// <summary>
    /// event의 optionTexts를 option 버튼에 표시한다.
    /// </summary>
    /// <param name="optionTexts"></param>
    private void NoticeOptions(ExplorationEvent @event)
    {
        List<string> optionTexts = @event.optionTexts;
        for(int i=0;i<optionTexts.Count;i++)
        {
            buttonOptions[i].interactable = @event.GetOptionEnable(i);
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
        for(int i=0; i<contentsEvent.Count - 1;i++)
        {
            contentsEvent[i].eventText.text = contentsEvent[i + 1].eventText.text;
            if(contentsEvent[i + 1].eventImage.IsActive())
            {
                contentsEvent[i].eventImage.gameObject.SetActive(true);
                contentsEvent[i].eventImage.sprite = contentsEvent[i + 1].eventImage.sprite;
                contentsEvent[i + 1].eventImage.gameObject.SetActive(false);
            }
            else
            {
                contentsEvent[i].eventImage.gameObject.SetActive(false);
            }
        }
        contentsEvent[contentsEvent.Count - 1].eventImage.sprite = null;
        contentsEvent[contentsEvent.Count - 1].eventImage.gameObject.SetActive(false);
        contentsEvent[contentsEvent.Count - 1].eventText.text = null;
    }
    #endregion

    #region Option Event Methods

    /// <summary>
    /// @event의 option을 선택했을 때 일어나는 일을 Button에 등록한다.
    /// </summary>
    /// <param name="event"></param>
    public void AddResultOptionsToButton(ExplorationEvent @event)
    {
        //AddResultTextsToButton(@event);
        AddResultEventsToButton(@event);
    }

    /// <summary>
    /// option을 선택했을 때 event의 결과를 Button에 등록한다.
    /// </summary>
    private void AddResultEventsToButton(ExplorationEvent @event)
    {
        if (@event.OptionNumber >= 0)
        {
            buttonOptions[0].onClick.AddListener(@event.Option0);
            if (@event.OptionNumber >= 1)
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
    /// 선택지가 선택되었을 때 발생하는 결과 텍스트 출력 이벤트를 버튼에 등록한다.
    /// </summary>
    private void AddResultTextsToButton(ExplorationEvent @event)
    {
        //if (@event.phase != ExplorationEvent.EventPhase.FinishingExploration && !ExplorationManager.Inst.GetIsOverwork())
        if (@event.phase != ExplorationEvent.EventPhase.FinishingExploration)
        {
            if (@event.OptionNumber >= 0)
            {
                buttonOptions[0].onClick.AddListener(() => NoticeEventText(@event.optionResultTexts[0]));
                if (@event.OptionNumber >= 1)
                {
                    buttonOptions[1].onClick.AddListener(() => NoticeEventText(@event.optionResultTexts[1]));
                    if (@event.OptionNumber >= 2)
                    {
                        buttonOptions[2].onClick.AddListener(() => NoticeEventText(@event.optionResultTexts[2]));
                        if (@event.OptionNumber >= 3)
                        {
                            buttonOptions[3].onClick.AddListener(() => NoticeEventText(@event.optionResultTexts[3]));
                        }
                    }
                }
            }
        }
    }

    public void NoticeResultText(string resultText)
    {
        NoticeEventText(resultText);
    }

    /// <summary>
    /// option 버튼에 등록되어 있는 모든 이벤트를 제거한다. 버튼을 비활성화 한다.
    /// </summary>
    public void RemoveEventsFromButton()
    {
        for(int i=0;i<buttonOptions.Length;i++)
        {
            buttonOptions[i].interactable = false;
            buttonOptions[i].onClick.RemoveAllListeners();
        }
    }

    #endregion

    public IEnumerator WaitForEncounter(float timeInterval)
    {
        NoticeEventText("탐사중.");
        yield return new WaitForSeconds(timeInterval / 3);
        contentsEvent[contentsEvent.Count - 1].eventText.text += ".";
        yield return new WaitForSeconds(timeInterval / 3);
        contentsEvent[contentsEvent.Count - 1].eventText.text += ".";
    }

    public void ActiveCollapseWarningPanel(FinishExplorationEvent finishExplorationEvent, int option)
    {
        panelCollapseWarning.SetActive(true);
        ButtonCancel.onClick.AddListener(CloseCooapseWarningPanel);
        if (option == 0)
        {
            ButtonContinue.onClick.AddListener(finishExplorationEvent.ExploreAnother);
            ButtonContinue.onClick.AddListener(() => NoticeEventText(finishExplorationEvent.optionResultTexts[0]));
        }
        else if (option == 1)
        {
            ButtonContinue.onClick.AddListener(finishExplorationEvent.ExploreAgain);
            ButtonContinue.onClick.AddListener(() => NoticeEventText(finishExplorationEvent.optionResultTexts[1]));
        }
        ButtonContinue.onClick.AddListener(CloseCooapseWarningPanel);
    }

    public void CloseCooapseWarningPanel()
    {
        panelCollapseWarning.SetActive(false);
        ButtonContinue.onClick.RemoveAllListeners();
        ButtonCancel.onClick.RemoveAllListeners();
    }

    #endregion

}
