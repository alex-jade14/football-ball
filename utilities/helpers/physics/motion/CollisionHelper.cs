using Godot;

public partial class CollisionHelper
{
    public static Vector3 CalculateNewVelocityFromCoefficientOfRestitution(float coefficientOfRestitution, Vector3 collisionNormal, Vector3 linearVelocity){
        float velocityAlongNormal = linearVelocity.Dot(collisionNormal);
        Vector3 newLinearVelocity = linearVelocity - (1 + coefficientOfRestitution) * velocityAlongNormal * collisionNormal;
        return newLinearVelocity;
    }

    public static Vector3 CalculateNewAngularVelocityFromFrictionWhenBouncing(Vector3 linearVelocity, Vector3 previouslinearVelocity, Vector3 angularVelocity,
    Vector3 collisionPosition, Vector3 collisionNormal, float radius, float mass, float inertia, float frictionCoefficient){
        Vector3 tangentVelocity = previouslinearVelocity - (previouslinearVelocity.Dot(collisionNormal)) * collisionNormal;
        Vector3 auxE = collisionPosition.Cross(angularVelocity) - tangentVelocity;
        Vector3 e = auxE / auxE.Length();
        Vector3 f = collisionPosition.Cross(e)/radius;
        float time = 0.1f;
        if(linearVelocity.Length() <= 10){
            time = 0.25f;
        }
        float Fn = 2 * mass * (previouslinearVelocity.Normalized().Length() / time);
        Vector3 Ft = frictionCoefficient * Fn * e;
        angularVelocity +=  f * ((Ft.Length() * radius) / inertia) * PhysicsServerHelper.DeltaFromPhysicsProcess;
        return angularVelocity;
    }
}