using Godot;
using System;

public partial class CollisionHelper
{
    public static (Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity) CalculateNewVelocityFromCoefficientOfRestitution(float coefficientOfRestitution,
    float rotationalCoefficientOfRestitution, float mass, Godot.Vector3 collisionNormal, Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity){
        float velocityAlongNormal = linearVelocity.Dot(collisionNormal);
        Godot.Vector3 newLinearVelocity = linearVelocity - (1 + coefficientOfRestitution) * velocityAlongNormal * collisionNormal;
        float angularVelocityAlongNormal = angularVelocity.Dot(collisionNormal);
        Godot.Vector3 newAngularVelocity = angularVelocity - (1 + rotationalCoefficientOfRestitution) * angularVelocityAlongNormal * collisionNormal;
        return (newLinearVelocity, newAngularVelocity);
    }

    public static Godot.Vector3 BounceVelocity(Godot.Vector3 linearVelocity, float coefficientOfRestitution, Godot.Vector3 collisionNormal){
        return linearVelocity.Bounce(coefficientOfRestitution * collisionNormal);
    }
}