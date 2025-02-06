using Godot;
using System;

public partial class CollisionHelper
{
    public static Godot.Vector3 CalculateNewVelocityFromCoefficientOfRestitution(float coefficientOfRestitution,
    float mass, Godot.Vector3 collisionNormal, Godot.Vector3 linearVelocity){
        float velocityAlongNormal = linearVelocity.Dot(collisionNormal);
        Godot.Vector3 newLinearVelocity = linearVelocity - (1 + coefficientOfRestitution) * velocityAlongNormal * collisionNormal;
        return newLinearVelocity;
    }

    public static (Vector3 linearVelocity, Vector3 angularVelocity, bool canRoll) CalculateNewVelocityFromFrictionWhenBouncing(Vector3 frictionForce, Vector3 upDirection, float mass,
    Vector3 linearVelocity, Vector3 angularVelocity, bool couldNotBeBouncing, float radius, Godot.Vector3 collisionPosition, float inertia, float frictionCoefficient, Godot.Vector3 collisionNormal, bool canRoll){
        Vector3 previousLinearVelocity = linearVelocity;
        linearVelocity = FrictionHelper.CalculateLinearVelocityFromFriction(linearVelocity, frictionForce, mass);
        if(couldNotBeBouncing){
            if(linearVelocity.Length() - (angularVelocity.Length() * radius) < 0.01){
                Vector3 newAngularVelocity = FrictionHelper.CalculateAngularVelocityFromFriction(linearVelocity, upDirection, radius);
                angularVelocity = angularVelocity.Lerp(newAngularVelocity, 0.5f);
                canRoll = true;
            }
        }
        else{
            canRoll = false;
            Vector3 tangentVelocity = previousLinearVelocity - (previousLinearVelocity.Dot(collisionNormal)) * collisionNormal;
            Vector3 auxE = collisionPosition.Cross(angularVelocity) - tangentVelocity;
            Vector3 e = auxE / auxE.Length();
            Vector3 f = collisionPosition.Cross(e)/radius;
            float time = 0.1f;
            if(linearVelocity.Length() <= 10){
                time = 0.25f;
            }
            float Fn = 2 * mass * (previousLinearVelocity.Normalized().Length() / time);
            Vector3 Ft = frictionCoefficient * Fn * e;
            angularVelocity +=  f * ((Ft.Length() * radius) / inertia) * PhysicsServerHelper.deltaFromPhysicsProcess;
        }
        return (linearVelocity, angularVelocity, canRoll);
    }
}