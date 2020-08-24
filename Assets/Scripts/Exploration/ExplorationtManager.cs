using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationtManager : SingletonBehaviour<ExplorationtManager>
{
    // for debugging
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

    /// <summary>
    /// @event를 등장시킨다.
    /// </summary>
    /// <param name="event"></param>
    private void AppearEvent(ExplorationEvent @event)
    {
        objectState = ObjectState.EventIsAppeared;
        ExplorationUIManager.Inst.RemoveEventsFromButton();
        _currentEvent = @event;
        ExplorationUIManager.Inst.NoticeEvent(_currentEvent);
        ExplorationUIManager.Inst.AddResultOptionsToButton(_currentEvent);
    }

    private float time = 0.0f;
    private ExplorationEvent _nextEvent;
    /// <summary>
    /// 다음으로 등장할 Event를 선택하고 등장시킨다.
    /// </summary>
    public void SelectNextEvent()
    {
        objectState = ObjectState.SearchNextEvent;
        StartCoroutine(ExplorationUIManager.Inst.WaitforEncounter());
        _nextEvent = _events[Random.Range(0, _events.Count)];
    }

    private void Update()
    {
        if(objectState == ObjectState.SearchNextEvent)
        {
            time += UnityEngine.Time.deltaTime;
            if (time > 3.0f)
            {
                AppearEvent(_nextEvent);
                time = 0.0f;
            }
        }
    }
}
