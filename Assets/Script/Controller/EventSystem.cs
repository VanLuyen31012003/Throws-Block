using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EventSystem
{
    private static Dictionary<string, Action> events = new Dictionary<string, Action>();

    public static void Subscribe(string eventName, Action listener)
    {
        if (events.ContainsKey(eventName))
            events[eventName] += listener;
        else
            events[eventName] = listener;
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (events.ContainsKey(eventName))
            events[eventName] -= listener;
    }

    public static void Trigger(string eventName)
    {
        if (events.ContainsKey(eventName))
            events[eventName]?.Invoke();
    }
}
