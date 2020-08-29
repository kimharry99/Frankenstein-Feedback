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
        unlockedRegions.Add(allRegions.Find(x => x.regionName == "도시"));
        //ChangeRegion(null, "도시");
        // for debugging
        unlockedRegions.Add(allRegions.Find(x => x.regionName == "더미지역"));
        ChangeRegion(null, "동굴");
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
        if (region == null)
            _currentRegion = allRegions.Find(x => (x.regionId == regionId || x.regionName == regionName));
        else
        {
            if (unlockedRegions.Contains(region))
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
        if(!unlockedRegions.Contains(foundRegion))
            unlockedRegions.Add(foundRegion);
    }

    /// <summary>
    /// 다른 지역으로 이동한다.
    /// </summary>
    public void MoveToAnotherRegion(Region linkedRegion)
    {
        UnlockRegion(linkedRegion);
        ChangeRegion(linkedRegion);
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
    /// <param name="event">현재 이벤트로 결정된 이벤트</param>
    public void SelectEvent(ExplorationEvent @event = null)
    {
        if(@event != null)
        {
            _currentEvent = @event;
            return;
        }
        if (phaseState == PhaseState.FinishingExploration)
        {
            Debug.LogError("wrong case");
            _currentEvent = _currentRegion.finishExplorationEvents[0];
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

    private List<ExplorationEvent> _enableEvents;
    /// <summary>
    /// events 배열에서 적절한 확률에 맞게 이벤트를 선택한다.
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    private ExplorationEvent SelectEventFromArray(ExplorationEvent[] events)
    {
        _enableEvents = FilterEnableEvent(events);
        int[] cumulativeProbability = MakeCumulativeProbability(_enableEvents);
        int temp = (int)(UnityEngine.Time.time * 100.0f);
        Random.InitState(temp);
        int random = Random.Range(0, 100);
        return GetSelectedEvent(random, cumulativeProbability, _enableEvents);
    }

    private List<ExplorationEvent> FilterEnableEvent(ExplorationEvent[] events)
    {
        List<ExplorationEvent> enableEvents = new List<ExplorationEvent>();
        for(int i=0;i<events.Length;i++)
        {
            if (events[i].phase == ExplorationEvent.EventPhase.RandomEncounter)
            {
                RandomEncounterEvent randomEncounter = (RandomEncounterEvent)events[i];
                if (randomEncounter.IsEnabled)
                {
                    enableEvents.Add(events[i]);
                }
            }
            else
            {
                if (events[i].IsEnabled)
                {
                    enableEvents.Add(events[i]);
                }
            }
        }
        return enableEvents;
    }

    /// <summary>
    /// event 배열을 참조하여 누적 확률 배열을 만든다.
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    private int[] MakeCumulativeProbability(List<ExplorationEvent> events)
    {
        int[] cumulativeProbability = new int[events.Count + 1];
        cumulativeProbability[0] = 0;
        for(int i=1;i<=events.Count;i++)
        {
            cumulativeProbability[i] = cumulativeProbability[i - 1] + (int)events[i - 1].encounterProbabilty;
        }
        return cumulativeProbability;
    }

    private ExplorationEvent GetSelectedEvent(int random, int[] cumlativeProbability, List<ExplorationEvent> events)
    {
        if(random >= cumlativeProbability.Last())
        {
            if (phaseState == PhaseState.ItemDiscovery1 || phaseState == PhaseState.ItemDiscovery2)
                return _itemDiscoveryFaultEvent;
            else if (phaseState == PhaseState.RandomEncounter)
                return _currentRegion.skipRandomEncounterEvent;
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
    /// <param name="nextEvent">nextEvent가 있는 경우에 nextEvent를 실행, RandomEncounter임에도 nextEvent가 없는 경우에 랜덤 인카운터를 한 번더 실행</param>
    /// <param name="isReturnHome"></param>
    public void FinishEvent(ExplorationEvent.EventPhase eventPhase, ExplorationEvent nextEvent = null, bool isReturnHome = false)
    {
        ExplorationUIManager.Inst.RemoveEventsFromButton();
        _currentEvent = null;
        switch (eventPhase)
        {
            case ExplorationEvent.EventPhase.SearchingItem:
                explorationCnt++;
                StartCoroutine(ExplorationUIManager.Inst.WaitForEncounter(_timeInterval));
                objectState = ObjectState.SearchNextEvent;
                ChangeToFollowingState();
                Debug.Log(phaseState);
                SelectEvent();
                break;
            case ExplorationEvent.EventPhase.RandomEncounter:
                explorationCnt++;
                if (nextEvent != null)
                {
                    SkipInterval();
                }
                else
                    StartCoroutine(ExplorationUIManager.Inst.WaitForEncounter(_timeInterval));
                objectState = ObjectState.SearchNextEvent;
                if(nextEvent != null)
                    if(nextEvent.phase != ExplorationEvent.EventPhase.RandomEncounter)
                        ChangeToFollowingState();
                Debug.Log(phaseState);
                SelectEvent(nextEvent);
                break;
            case ExplorationEvent.EventPhase.FinishingExploration:
                if(!isReturnHome)
                {
                    StartCoroutine(ExplorationUIManager.Inst.WaitForEncounter(_timeInterval));
                    objectState = ObjectState.SearchNextEvent;
                    ChangeToFollowingState();
                    Debug.Log(phaseState);
                    SelectEvent();
                }
                break;
        }
    }

    /// <summary>
    /// 연계되는 이벤트를 활성화한다.
    /// </summary>
    public void UnlockLinkedEvent(string linkedEventName)
    {
        if (linkedEventName != "")
        {
            for (int i = 0; i < _currentRegion.possibleRandomEncounterEvents.Length; i++)
            {
                if (_currentRegion.possibleRandomEncounterEvents[i].eventName == linkedEventName)
                {
                    _currentRegion.possibleRandomEncounterEvents[i].IsEnabled = true;
                    break;
                }
            }
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
