using Godot;
using System;
using System.Collections.Generic;

public partial class EventManager : ISubject
{
    public int state { get; set; }
    public GodotObject data { get; set; }
    private List<IObserver> _observers = new List<IObserver>();

    public EventManager(){}

    public void Attach(IObserver observer){
        _observers.Add(observer);
    }

    public void Detach(IObserver observer){
        _observers.Remove(observer);
    }

    public void Notify(GodotObject data){
        this.data = data;
        foreach(IObserver observer in _observers){
            observer.Update(this);
        }
    }

}