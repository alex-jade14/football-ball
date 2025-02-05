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
}