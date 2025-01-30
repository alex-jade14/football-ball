using Godot;
using Godot.Collections;
using System;

public interface IObserver
{
    void Update(EventManager manager);
}