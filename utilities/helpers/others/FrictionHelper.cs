using Godot;
using System;

public partial class FrictionHelper
{
    public static float CalculateFrictionForce(float frictionCoefficient, float normalForce){
        return frictionCoefficient * normalForce;
    }

    public static Vector3 CalculateAngularVelocityFromFriction(Vector3 linearVelocity, 
        Vector3 upDirection, float radius){
        float angularSpeed = linearVelocity.Length() / radius;
        Vector3 rotationAxis = upDirection.Cross(linearVelocity.Normalized()).Normalized();
        Vector3 newAngularVelocity = rotationAxis * angularSpeed;
        return newAngularVelocity;
    }

    public static Vector3 CalculateLinearVelocityFromFriction(Vector3 linearVelocity, Vector3 frictionForce, float mass){
        frictionForce.Y = Mathf.Abs(frictionForce.Y);
        Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(frictionForce, mass);
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        return linearVelocity;
    }
}