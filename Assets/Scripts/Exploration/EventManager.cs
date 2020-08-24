using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : SingletonBehaviour<EventManager>
{
    [SerializeField] // for debugging
    private List<ExplorationEvent> events;


    /// <summary>
    /// @event를 등장시킨다.
    /// </summary>
    /// <param name="event"></param>
    private void AppearEvent(ExplorationEvent @event)
    {
        
    }

    private void Foo()
    {
        Debug.Log("foo");
    }
}
