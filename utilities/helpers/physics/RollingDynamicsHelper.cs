using Godot;
using System;

public partial class RollingDynamicsHelper{
    public static float rotationReductionFactorBox = 5;
    public static float interpolationCoefficient = 0.01f;
    public static float hola = 0;
    public static float hola2 = 1;

    public static (Vector3 linearVelocity, Vector3 angularVelocity) CalculateRollingWithoutSlipping(Vector3 frictionForce, Vector3 upDirection, float mass,
    Vector3 linearVelocity, Vector3 angularVelocity, bool canRoll, float radius, Godot.Vector3 collisionPosition, float inertia, float frictionCoefficient, Godot.Vector3 collisionNormal, float frictionValue){
        Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(frictionForce, mass);
        Vector3 previousLinearVelocity = linearVelocity;
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        if(canRoll){
            if(linearVelocity.Length() - angularVelocity.Length() * radius < 0.01){
                float angularSpeed = linearVelocity.Length() / radius;
                Vector3 rotationAxis = upDirection.Cross(linearVelocity.Normalized()).Normalized();
                Vector3 newAngularVelocity = rotationAxis * angularSpeed;
                angularVelocity = newAngularVelocity;
            }
        }
        else{
            Vector3 tangentVelocity = previousLinearVelocity - (previousLinearVelocity.Dot(collisionNormal)) * collisionNormal;
            Vector3 auxE = collisionPosition.Cross(angularVelocity) - tangentVelocity;
            Vector3 e = auxE / auxE.Length();
            Vector3 f = collisionPosition.Cross(e)/radius;
            float time = 0.1f;
            if(linearVelocity.Length() <= 10){
                time = 0.25f;
            }
            float Fn = 2 * mass * ((previousLinearVelocity.Normalized()).Length() / time);
            Vector3 Ft = frictionCoefficient * Fn * e;
            angularVelocity +=  f * ((Ft.Length() * radius) / inertia) * PhysicsServerHelper.deltaFromPhysicsProcess;
        }
        return (linearVelocity, angularVelocity);
    }

}