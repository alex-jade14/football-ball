using Godot;
using System;

public partial class FrictionHelper
{
    public static float CalculateFrictionForce(float frictionCoefficient, float normalForce){
        return frictionCoefficient * normalForce;
    }

    public static Vector3 CalculateLinearVelocityFromFriction(Vector3 linearVelocity, Vector3 frictionForce, float mass){
        frictionForce.Y = 0;
        Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(frictionForce, mass);
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration);
        return linearVelocity;
    }

    public static Vector3 CalculateAngularVelocityFromFriction(Vector3 linearVelocity, Vector3 upDirection, float radius){
        float angularSpeed = linearVelocity.Length() / radius;
        Vector3 rotationAxis = upDirection.Cross(linearVelocity.Normalized()).Normalized();
        Vector3 newAngularVelocity = rotationAxis * angularSpeed;
        return newAngularVelocity;
    }

    public static Vector3 CalculateNewAngularVelocityFromFrictionWhenBouncing(Vector3 linearVelocity, Vector3 previouslinearVelocity, Vector3 angularVelocity,
    Vector3 collisionPosition, Vector3 collisionNormal, float radius, float mass, float inertia, float frictionCoefficient){
        Vector3 tangentVelocity = previouslinearVelocity - (previouslinearVelocity.Dot(collisionNormal)) * collisionNormal;
        Vector3 auxE = collisionPosition.Cross(angularVelocity) - tangentVelocity;
        Vector3 e = auxE / auxE.Length();
        Vector3 f = collisionPosition.Cross(e)/radius;
        float time = 0.04f; //Arbitrary value
        if(linearVelocity.Length() <= 10){
            time = 0.25f; //Arbitrary value
        }
        float Fn = 2 * mass * (previouslinearVelocity.Normalized().Length() / time);
        Vector3 Ft = frictionCoefficient * Fn * e;
        Vector3 angularAcceleration = f * ((Ft.Length() * radius) / inertia);
        angularVelocity +=  MotionHelper.CalculateVelocityFromAcceleration(angularAcceleration);
        return angularVelocity;
    }
}