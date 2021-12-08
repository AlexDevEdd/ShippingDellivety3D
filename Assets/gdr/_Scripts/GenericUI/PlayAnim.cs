using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class PlayAnim : MonoBehaviour
{
    public string parameterName;

    private void Start()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((x) => { GetComponent<Animator>().SetTrigger(parameterName); });
        eventTrigger.triggers.Add(entry);
    }
}
