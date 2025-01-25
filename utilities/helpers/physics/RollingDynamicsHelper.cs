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
        float interpolationCoefficient = 0.05f;
        float magicNumber = 2;
        Godot.Vector3 newAngularVelocity = (((linearVelocity/circumference) * (2 * Mathf.Pi)).Cross(collisionPosition * magicNumber));
        if (linearVelocity.Length() > 0.1){
            angularVelocity = angularVelocity * (1.0f - interpolationCoefficient) + newAngularVelocity * interpolationCoefficient;
        }
        else{
            angularVelocity = newAngularVelocity;
        }

        return (linearVelocity, angularVelocity);
    }

}