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
    private ItemDiscoveryEvent[] _itemDiscoveryEvents;
    private ExplorationEvent[] _randomEncounterEvents;
    [SerializeField]
    private FinishExplorationEvent _finishExplorationEvent;
    [SerializeField]
    private ItemDiscoveryEvent _itemDiscoveryFaultEvent;

    private ObjectState objectState;
    private PhaseState phaseState;
    private int explorationCnt = 0;
    private List<Region> allRegions;
    private List<Region> unlockedRegions = new List<Region>();
    private Region _currentRegion;

    private void Start()
    {
        allRegions = Resources.LoadAll<Region>("Regions").ToList();
        unlockedRegions.Add(allRegions.Find(x => x.regionName == "더미"));
        ChangeRegion(null, "더미");
        // for debugging
        SelectEvent();
        SkipInterval();
    }

    public void InitializeExploration()
    {
        Debug.Log("탐사 초기화");
        phaseState = PhaseState.ItemDiscovery1;
        AppearEvent(_itemDiscoveryEvents[1]);
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
            _currentRegion = unlockedRegions.Find(x => (x.regionId == regionId || x.regionName == regionName));
        else
        {
            if(unlockedRegions.Contains(region))
            {
                _currentRegion = region;
            }
        }
        _itemDiscoveryEvents = _currentRegion.possibleItemDiscoveryEvents;
        _randomEncounterEvents = _currentRegion.possibleRandomEncounterEvents;
    }


    public void UnlockRegion(Region foundRegion)
    {
        Debug.Log("지역 발견 : " + foundRegion.regionName);   
        unlockedRegions.Add(foundRegion);
    }

    /// <summary>
    /// 다른 지역으로 이동한다.
    /// </summary>
    public void MoveToAnotherRegion()
    {
        Random.InitState((int)UnityEngine.Time.time);
        Region nextRegion = _currentRegion;
        for(int i=0;i<100;i++)
        {
            int random = Random.Range(0, unlockedRegions.Count);
            nextRegion = unlockedRegions[random];
            if (nextRegion.regionId != _currentRegion.regionId)
                break;
        }
        ChangeRegion(nextRegion);
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
        if (phaseState == PhaseState.FinishingExploration)
        {
            _currentEvent = _finishExplorationEvent;
        }
        else if (phaseState == PhaseState.ItemDiscovery1 || phaseState == PhaseState.ItemDiscovery2)
        {
            _currentEvent = SelectEventFromArray(_itemDiscoveryEvents);
        }
        else if (phaseState == PhaseState.RandomEncounter)
        { 
            _currentEvent = SelectEventFromArray(_randomEncounterEvents);
        }
    }

    /// <summary>
    /// events 배열에서 적절한 확률에 맞게 이벤트를 선택한다.
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    private ExplorationEvent SelectEventFromArray(ExplorationEvent[] events)
    {
        int[] cumulativeProbability = MakeCumulativeProbability(events);
        int temp = (int)(UnityEngine.Time.time * 100.0f);
        Random.InitState(temp);
        int random = Random.Range(0, 100);
        return GetSelectedEvent(random, cumulativeProbability, events);
    }

    /// <summary>
    /// event 배열을 참조하여 누적 확률 배열을 만든다.
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    private int[] MakeCumulativeProbability(ExplorationEvent[] events)
    {
        int[] cumulativeProbability = new int[events.Length + 1];
        cumulativeProbability[0] = 0;
        for(int i=1;i<=events.Length;i++)
        {
            cumulativeProbability[i] = cumulativeProbability[i - 1] + (int)events[i - 1].encounterProbabilty;
        }
        return cumulativeProbability;
    }

    private ExplorationEvent GetSelectedEvent(int random, int[] cumlativeProbability, ExplorationEvent[] events)
    {
        if(random >= cumlativeProbability.Last())
        {
            return _itemDiscoveryFaultEvent;
        }

        for(int i=cumlativeProbability.Length - 2;i>=0;i--)
        {
            if(random >= cumlativeProbability[i])
            {
                return events[i];
            }
        }
        return events[0];
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
    /// 탐사 횟수와 인내력을 비교한다.
    /// </summary>
    /// <returns></returns>
    public bool GetIsOverwork()
    {
        if (explorationCnt >= Player.Inst.Endurance)
        {
            return true;
        }
        else return false;
    }

    /// <summary>
    /// event선택이 종료된 후 후처리를 담당한다.
    /// </summary>
    /// <param name="isReturnHome"></param>
    public void FinishEvent(bool isReturnHome = false)
    {
        ExplorationUIManager.Inst.RemoveEventsFromButton();
        if(phaseState != PhaseState.FinishingExploration)
            explorationCnt++;
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
