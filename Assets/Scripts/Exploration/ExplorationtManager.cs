using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationtManager : SingletonBehaviour<ExplorationtManager>
{
    private enum ObjectState
    {
        None = -1,
        SearchNextEvent,
        EventIsAppeared,
    }
    ObjectState objectState;

    [SerializeField] // for debugging
    private List<ExplorationEvent> _events;

    private ExplorationEvent _currentEvent;

    protected override void Awake()
    {
        base.Awake();
        // for debugging
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
                SelectNextEvent();
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
        _currentEvent = @event;
        ExplorationUIManager.Inst.NoticeEvent(_currentEvent);
        ExplorationUIManager.Inst.AddResultOptionsToButton(_currentEvent);
    }

    [Header("Debugging Field")]
    [SerializeField]
    private float _timeInterval = 3.0f;
    private ExplorationEvent _nextEvent;
    /// <summary>
    /// 다음으로 등장할 Event를 선택하고 등장시킨다.
    /// </summary>
    public void SelectNextEvent()
    {
        int temp = (int) (UnityEngine.Time.time * 100.0f);
        Random.InitState(temp);
        _nextEvent = _events[Random.Range(0, _events.Count)];
        AppearEvent(_nextEvent);
    }

    public void FinishEvent()
    {
        GameManager.Inst.OnTurnOver(1);

        ExplorationUIManager.Inst.RemoveEventsFromButton();
        _currentEvent = null;
        StartCoroutine(ExplorationUIManager.Inst.WaitForEncounter(_timeInterval));
        objectState = ObjectState.SearchNextEvent;
    }
}
