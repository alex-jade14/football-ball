using Godot;
using System;

public partial class MotionHelper
{
    public static Godot.Vector3 CalculateVelocityFromAcceleration(Godot.Vector3 acceleration, float delta){
        return acceleration * delta;
    }

}