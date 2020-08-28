using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 탐사시에 방문하는 지역이다.
/// </summary>
[CreateAssetMenu]
public class Region : ScriptableObject
{
    public int regionId;
    public string regionName;
    public ItemDiscoveryEvent[] possibleItemDiscoveryEvents;
    public ExplorationEvent[] possibleRandomEncounterEvents;
    public SkipEvent skipRandomEncounterEvent;
    public FinishExplorationEvent[] finishExplorationEvents;
}
