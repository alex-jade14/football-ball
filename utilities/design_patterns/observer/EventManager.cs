using Godot;
using System;
using System.Collections.Generic;

public partial class EventManager : ISubject
{
    public Godot.Collections.Dictionary data { get; set; }
    public List<Tuple<String, IObserver>> _observers = new();

    public EventManager(){

    }

    public void Attach(String eventType, IObserver observer){
        _observers.Add(new Tuple<String, IObserver>(eventType, observer));
    }

    public void Detach(String eventType, IObserver observer){
        _observers.Remove(new Tuple<String, IObserver>(eventType, observer));
    }

    public void Notify(String eventType, Godot.Collections.Dictionary data){
        this.data = data;
        foreach(Tuple<String, IObserver> observer in _observers){
            if(observer.Item1 == "impulse"){
                observer.Item2.UpdateByImpulse(data);
            }
            else if(observer.Item1 == "detectedCollision"){
                observer.Item2.UpdateByDetectedCollision(data);
            }
            else if(observer.Item1 == "updateMarker"){
                observer.Item2.UpdateShadowBallMarker(data);
            }
        }
    }

}