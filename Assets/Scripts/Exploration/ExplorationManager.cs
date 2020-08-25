using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager : SingletonBehaviour<ExplorationManager>
{
    private enum ObjectState
    {
        None = -1,
        SearchNextEvent,
        EventIsAppeared,
    }

    private enum PhaseState
    {
        None = -1,
        ItemDiscovery1,
        ItemDiscovery2,
        RandomEncounter,
        FinishingExploration,
    }

    [SerializeField] // for debugging
    private List<ExplorationEvent> _events;

    private ObjectState objectState;
    private PhaseState phaseState;
    private ExplorationEvent.Region _curentRegion;
    private ExplorationEvent _1currentEvent;

    private void Start()
    {
        // for debugging
        //InitializeExploration();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeExploration()
    {
        Debug.Log("탐사 초기화");
        phaseState = PhaseState.ItemDiscovery1;
        _curentRegion = ExplorationEvent.Region.City;
        AppearEvent(_events[1]);
    }

    private float _time = 0.0f;
    private void Update()
    {
        if (objectState == ObjectState.SearchNextEvent)
        {
            _time += UnityEngine.Time.deltaTime;
            if (_time > _timeInterval)
            {
                SelectEvent();
                _time = 0.0f;
            }
        }
    }

    /// <summary>
    /// @event를 등장시킨다.
    /// </summary>
    /// <param name="event"></param>
    private void AppearEvent(ExplorationEvent @event)
    {
        objectState = ObjectState.EventIsAppeared;
        ExplorationUIManager.Inst.NoticeEvent(@event);
        ExplorationUIManager.Inst.AddResultOptionsToButton(@event);
    }

    [Header("Debugging Field")]
    [SerializeField]
    private float _timeInterval = 3.0f;
    private ExplorationEvent _currentEvent;
    /// <summary>
    /// 다음으로 등장할 Event를 선택하고 등장시킨다.
    /// </summary>
    public void SelectEvent()
    {
        int temp = (int) (UnityEngine.Time.time * 100.0f);
        Random.InitState(temp);
        _currentEvent = _events[Random.Range(0, _events.Count)];
        AppearEvent(_currentEvent);
    }

    /// <summary>
    /// event선택이 종료된 후 후처리를 담당한다.
    /// </summary>
    /// <param name="isReturnHome"></param>
    public void FinishEvent(bool isReturnHome = false)
    {
        Debug.Log("이벤트 종료");
        ExplorationUIManager.Inst.RemoveEventsFromButton();
        _1currentEvent = null;
        if (!isReturnHome)
        {
            StartCoroutine(ExplorationUIManager.Inst.WaitForEncounter(_timeInterval));
            objectState = ObjectState.SearchNextEvent;
        }
        else
        {
            GameManager.Inst.ReturnHome();
        }
        GameManager.Inst.OnTurnOver(1);
    }
}
