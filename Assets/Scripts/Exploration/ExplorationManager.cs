using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private ItemDiscoveryEvent _itemDiscoveryFaultEvent;

    private ObjectState objectState;
    private PhaseState phaseState;
    private List<Region> allRegions;
    private List<Region> unlockedRegions = new List<Region>();
    private Region _curentRegion;

    private void Start()
    {
        allRegions = Resources.LoadAll<Region>("Regions").ToList();
        unlockedRegions.Add(allRegions.Find(x => x.regionName == "더미"));
        unlockedRegions.Add(allRegions.Find(x => x.regionName == "더미학교"));
        ChangeRegion(null, "더미학교");
        // for debugging
        SelectEvent();
        SkipInterval();
    }

    public void InitializeExploration()
    {
        Debug.Log("탐사 초기화");
        phaseState = PhaseState.ItemDiscovery1;
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
    /// 이벤트 종료 후 대기시간을 skip한다.
    /// </summary>
    private void SkipInterval()
    {
        _time = _timeInterval + 2.0f;
    }

    public void ChangeRegion(Region region = null, string regionName = "", int regionId = 0)
    {
        if(region == null)
            _curentRegion = unlockedRegions.Find(x => (x.regionId == regionId || x.regionName == regionName));
        else
        {
            if(unlockedRegions.Contains(region))
            {
                _curentRegion = region;
            }
        }
        _events = _curentRegion.possibleEvents;
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
    /// 다음으로 등장할 Event를 선택한다.
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

    private bool CheckPhaseMatched(ExplorationEvent @event)
    {
        switch(@event.phase)
        {
            case ExplorationEvent.EventPhase.SearchingItem:
                if (phaseState == PhaseState.ItemDiscovery1 || phaseState == PhaseState.ItemDiscovery2)
                    return true;
                break;
            case ExplorationEvent.EventPhase.RandomEncounter:
                if (phaseState == PhaseState.RandomEncounter)
                    return true;
                break;
            case ExplorationEvent.EventPhase.FinishingExploration:
                if (phaseState == PhaseState.FinishingExploration)
                    return true;
                break;
        }
        return false;
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
