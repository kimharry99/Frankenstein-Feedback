using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationtManager : SingletonBehaviour<ExplorationtManager>
{
    [SerializeField] // for debugging
    private List<ExplorationEvent> _events;

    private ExplorationEvent _currentEvent;

    protected override void Awake()
    {
        base.Awake();
        // for debugging
        SelectNextEvent();
    }

    /// <summary>
    /// @event를 등장시킨다.
    /// </summary>
    /// <param name="event"></param>
    private void AppearEvent(ExplorationEvent @event)
    {
        ExplorationUIManager.Inst.RemoveEventsFromButton();
        _currentEvent = @event;
        ExplorationUIManager.Inst.NoticeEvent(_currentEvent);
        ExplorationUIManager.Inst.AddResultOptionsToButton(_currentEvent);
    }

    /// <summary>
    /// 다음으로 등장할 Event를 선택하고 등장시킨다.
    /// </summary>
    public void SelectNextEvent()
    {
        AppearEvent(_events[Random.Range(0, _events.Count)]);
    }

}
