using System;
using System.Collections.Generic;

public partial class EventManager : ISubject
{
    public List<Tuple<string, IObserver>> Observers = new();

    public void Attach(string eventType, IObserver observer){
        Observers.Add(new Tuple<string, IObserver>(eventType, observer));
    }

    public void Detach(string eventType, IObserver observer){
        Observers.Remove(new Tuple<string, IObserver>(eventType, observer));
    }

    public void Notify(string eventType, Godot.Collections.Dictionary data){
        foreach(Tuple<string, IObserver> observer in Observers){
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