using Godot;
using System;

public partial class RollingDynamicsHelper{
    public static float rotationReductionFactorBox = 4;
    public static float interpolationCoefficient = 0.1f;

    public static (Vector3 linearVelocity, Vector3 angularVelocity) CalculateRollingWithoutSlipping(
        Vector3 force, Vector3 collisionPosition, float mass, Vector3 linearVelocity, Vector3 angularVelocity, bool canRoll){
        Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(force, mass);
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        if(canRoll){
            Vector3 newAngularVelocity = collisionPosition.Cross(linearVelocity) / Mathf.Pow(collisionPosition.Length(), 2);
            angularVelocity = newAngularVelocity;
        }
        return (linearVelocity, angularVelocity);
    }

}