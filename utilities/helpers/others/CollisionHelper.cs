using Godot;
using System;

public partial class CollisionHelper
{
    public static Godot.Vector3 CalculateForceFromCoefficientOfRestitution(float coefficientOfRestitution,
    Godot.Vector3 collisionNormal, float mass, Godot.Vector3 linearVelocity){
        Godot.Vector3 newLinearVelocity = (-coefficientOfRestitution * collisionNormal) * linearVelocity;
        Godot.Vector3 newAcceleration = (newLinearVelocity - linearVelocity);
        Godot.Vector3 newForce = new Godot.Vector3(
            NewtonsSecondLawHelper.CaulcateForceWithAcceleration(mass, newAcceleration.X),
            NewtonsSecondLawHelper.CaulcateForceWithAcceleration(mass, newAcceleration.Y),
            NewtonsSecondLawHelper.CaulcateForceWithAcceleration(mass, newAcceleration.Z)
        );
        return newForce;
    }

    public static Godot.Vector3 BounceVelocity(Godot.Vector3 linearVelocity, float coefficientOfRestitution, Godot.Vector3 collisionNormal){
        return linearVelocity.Bounce(coefficientOfRestitution * collisionNormal);
    }
}