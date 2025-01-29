using Godot;
using System;

public partial class RollingDynamicsHelper{
    public static float delta = PhysicsServerHelper.deltaFromPhysicsProcess;

    public static (Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity)CalculateRollingWithoutSlipping(
        Godot.Vector3 force, Godot.Vector3 collisionPosition, float mass, float circumference, 
        Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity, bool canRoll){
        Godot.Vector3 acceleration = new(
            NewtonsSecondLawHelper.CalculateAccelerationWithForce(force.X, mass),
            NewtonsSecondLawHelper.CalculateAccelerationWithForce(force.Y, mass),
            NewtonsSecondLawHelper.CalculateAccelerationWithForce(force.Z, mass)
        );
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, delta);
        if(canRoll){
            // (linearVelocity / 4) -> 4 is a magic number
            Godot.Vector3 newAngularVelocity = collisionPosition.Cross(linearVelocity / 4) / Mathf.Pow(collisionPosition.Length(), 2);
            if (linearVelocity.Length() > 0.1){
                float interpolationCoefficient = 0.025f;
                angularVelocity = angularVelocity * (1.0f - interpolationCoefficient) + newAngularVelocity * interpolationCoefficient;
            }
            else{
                angularVelocity = newAngularVelocity;
            }
        }
        return (linearVelocity, angularVelocity);
    }

}