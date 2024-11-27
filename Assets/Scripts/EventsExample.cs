using System;
using UnityEngine;
using UnityEngine.Events;

public class EventsExample : MonoBehaviour
{
    // UnityEvents appear in the inspector
    // you can also use [HideInInspector] to hide UnityEvents
    public UnityEvent<EventArgsExample> UnityEventExample;

    // C# events NEVER appear in the inspector
    // we're using the EventHandler method for C# events
    // there are many different ways of using C# events, you might also see delegate or actions
    public event EventHandler<EventArgsExample> CSharpEventExample;

    public void RaiseEvents(EventArgsExample args)
    {
        // we fire UnityEvents using the Invoke method, and pass in arguments
        // UnityEvents do a null check internally, we don't have to worry about doing one ourselves
        // but many people do anyway
        UnityEventExample.Invoke(args);

        // we always have to null check C# events, if nothing is listening to them, they'll throw a null reference error
        CSharpEventExample?.Invoke(this, args);
    }

    public void AddListeners()
    {
        // add listener, or subscribe, to a UnityEvent
        UnityEventExample.AddListener(UnityEventListener);

        // same thing, but with a C# event, we use the += operator instead
        CSharpEventExample += CSharpEventListener;
    }

    private void CSharpEventListener(object sender, EventArgsExample e)
    {
        
    }

    private void UnityEventListener(EventArgsExample arg0)
    {
        
    }

    public void RemoveListeners()
    {
        // remove listener from UnityEvent
        UnityEventExample.RemoveListener(UnityEventListener);
        // don't use this, classes should be removing their own listeners
        // it's impossible to guess everything you'll break when you use this
        // UnityEventExample.RemoveAllListeners();

        // remove listenr from C# event
        CSharpEventExample -= CSharpEventListener;
    }
}

public class EventArgsExample
{
    public string Name { get; set; }
    public GameObject Target { get; set; }
    public bool IsValid { get; set; }
}