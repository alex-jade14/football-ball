using Godot;
using Godot.Collections;
using System;

public interface IObserver
{
    void UpdateByImpulse(Dictionary data);
    void UpdateByDetectedCollision(Dictionary data);
    void UpdateShadowBallMarker(Dictionary data);
}