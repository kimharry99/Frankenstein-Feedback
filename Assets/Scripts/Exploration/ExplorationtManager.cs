using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationtManager : SingletonBehaviour<ExplorationtManager>
{
    [SerializeField] // for debugging
    private List<ExplorationEvent> events;

    private ExplorationEvent currentEvent;

    protected override void Awake()
    {
        base.Awake();
        // for debugging
        AppearEvent(events[0]);
    }

    /// <summary>
    /// @event를 등장시킨다.
    /// </summary>
    /// <param name="event"></param>
    private void AppearEvent(ExplorationEvent @event)
    {
        ExplorationUIManager.Inst.RemoveEventsFromButton();
        currentEvent = @event;
        ExplorationUIManager.Inst.NoticeEvent(currentEvent);
        ExplorationUIManager.Inst.AddEventsToButton(@event);
    }

}
