using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentDayInfo", menuName = "Internal/CurrentDayInfo")]
public class CurrentTimeInfos : ScriptableObject
{
    public int currentDay;
    public DAY_PHASE currentPhase;
}
