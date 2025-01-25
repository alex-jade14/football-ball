using Godot;
using System;

public partial class RotationalMotionHelper
{

    public static Godot.Vector3 CalculateTorque(Godot.Vector3 position, Godot.Vector3 force){
        return position.Cross(force);
    }
    public static float CalculateInertiaFromSphere(float mass, float radius){
        return (2f/5f) * mass * Mathf.Pow(radius, 2f);
    }
}