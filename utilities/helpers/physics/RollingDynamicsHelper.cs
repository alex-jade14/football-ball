using Godot;
using System;

public partial class RollingDynamicsHelper{
    public static float delta = PhysicsServerHelper.deltaFromPhysicsProcess;

    public static (Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity)CalculateRollingWithoutSlipping(
        Godot.Vector3 force, Godot.Vector3 collisionPosition, float mass, float circumference, 
        Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity){
        Godot.Vector3 acceleration = new(
            NewtonsSecondLawHelper.CalculateAccelerationWithForce(force.X, mass),
            NewtonsSecondLawHelper.CalculateAccelerationWithForce(force.Y, mass),
            NewtonsSecondLawHelper.CalculateAccelerationWithForce(force.Z, mass)
        );
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, delta);
        Godot.Vector3 newAngularVelocity = (((linearVelocity/circumference) * (2 * Mathf.Pi)).Cross(collisionPosition));
        angularVelocity = newAngularVelocity;
        return (linearVelocity, angularVelocity);
    }

}