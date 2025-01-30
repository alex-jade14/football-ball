using Godot;
using System;

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(Godot.Collections.Dictionary data);

    
}