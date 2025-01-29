using Godot;
using System;

public partial class CollisionHelper
{
    public static Godot.Vector3 CalculateImpulseFromCoefficientOfRestitution(float coefficientOfRestitution,
    float mass, Godot.Vector3 linearVelocity){
        float newYLinearVelocity =  coefficientOfRestitution * Mathf.Abs(linearVelocity.Y);
        Godot.Vector3 newLinearVelocity = new(linearVelocity.X, newYLinearVelocity, linearVelocity.Z);
        Godot.Vector3 newAcceleration = newLinearVelocity - linearVelocity;
        Godot.Vector3 newForce = new Godot.Vector3(
            NewtonsSecondLawHelper.CalculateForceWithAcceleration(mass, newAcceleration.X),
            NewtonsSecondLawHelper.CalculateForceWithAcceleration(mass, newAcceleration.Y),
            NewtonsSecondLawHelper.CalculateForceWithAcceleration(mass, newAcceleration.Z)
        );
        return newForce;
    }

    public static Godot.Vector3 BounceVelocity(Godot.Vector3 linearVelocity, float coefficientOfRestitution, Godot.Vector3 collisionNormal){
        return linearVelocity.Bounce(coefficientOfRestitution * collisionNormal);
    }
}