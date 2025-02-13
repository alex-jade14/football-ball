using Godot;

public partial class CollisionHelper
{
    // Reference: https://www.youtube.com/watch?v=jH2TpEjMAl4
    // Reference: https://www.youtube.com/watch?v=90U_83QE1b8
    public static Vector3 CalculateNewVelocityFromCoefficientOfRestitution(float coefficientOfRestitution, Vector3 collisionNormal, Vector3 linearVelocity){
        float velocityAlongNormal = linearVelocity.Dot(collisionNormal);
        Vector3 newLinearVelocity = linearVelocity - (1 + coefficientOfRestitution) * velocityAlongNormal * collisionNormal;
        return newLinearVelocity;
    }
}