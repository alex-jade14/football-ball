using Godot.Collections;

public interface IObserver
{
    void UpdateByImpulse(Dictionary data);
    void UpdateByDetectedCollision(Dictionary data);
    void UpdateShadowBallMarker(Dictionary data);
}