using Godot;
using System;

public partial class RigidBodyHelper
{
    public static float delta = PhysicsServerHelper.deltaFromPhysicsProcess;

    public static Godot.Vector3 CalculateAngularDamping(float angularDampingCoefficient, Godot.Vector3 angularVelocity){
        float angularResult = 1.0f - PhysicsServerHelper.deltaFromPhysicsProcess * angularDampingCoefficient;
        if (angularResult < 0){
            angularResult = 0;
        }
        angularVelocity *= angularResult;
        return angularVelocity;
    }
}