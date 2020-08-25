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
    [SerializeField]
    private FinishExplorationEvent _finishExplorationEvent;

    private ObjectState objectState;
    private PhaseState phaseState;
    private ExplorationEvent.Region _curentRegion;

    private void Start()
    {
        // for debugging
        _currentEvent = _events[1];
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
                AppearEvent(_currentEvent);
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
        if (phaseState != PhaseState.ItemDiscovery1 && phaseState != PhaseState.FinishingExploration)
            GameManager.Inst.OnTurnOver(1);
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
        if(phaseState == PhaseState.FinishingExploration)
        {
            _currentEvent = _finishExplorationEvent;
        }
        else
        {
            int temp = (int)(UnityEngine.Time.time * 100.0f);
            Random.InitState(temp);
            _currentEvent = _events[Random.Range(0, _events.Count)];
        }
    }

    /// <summary>
    /// event선택이 종료된 후 후처리를 담당한다.
    /// </summary>
    /// <param name="isReturnHome"></param>
    public void FinishEvent(bool isReturnHome = false)
    {
        ExplorationUIManager.Inst.RemoveEventsFromButton();
        _currentEvent = null;
        if (!isReturnHome)
        {
            if (phaseState != PhaseState.RandomEncounter)
            {
                StartCoroutine(ExplorationUIManager.Inst.WaitForEncounter(_timeInterval));
            }
            else
            {
                _time = _timeInterval + 2.0f;
            }
            objectState = ObjectState.SearchNextEvent;
            ChangeToFollowingState();
            SelectEvent();
        }
    }

    /// <summary>
    /// 다음 탐사 State로 변경한다.
    /// </summary>
    private void ChangeToFollowingState()
    {
        if (phaseState == PhaseState.FinishingExploration)
        {
            phaseState = PhaseState.ItemDiscovery1;
        }
        else
            phaseState += 1;
    }
}
