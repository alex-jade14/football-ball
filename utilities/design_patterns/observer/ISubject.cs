using Godot;
using System;

public interface ISubject
{
    void Attach(String eventType, IObserver observer);
    void Detach(String eventType, IObserver observer);
    void Notify(String eventType, Godot.Collections.Dictionary data);

    
}