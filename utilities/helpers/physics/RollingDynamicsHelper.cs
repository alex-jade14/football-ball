using Godot;
using System;

public partial class RollingDynamicsHelper{
    public static (Vector3 linearVelocity, Vector3 angularVelocity) CalculateRollingWithoutSlipping(Vector3 linearVelocity,
    Vector3 frictionForce, Vector3 upDirection, float mass, float radius){
        //linearVelocity = FrictionHelper.CalculateLinearVelocityFromFriction(linearVelocity, frictionForce, mass);
        Vector3 angularVelocity = FrictionHelper.CalculateAngularVelocityFromFriction(linearVelocity, upDirection, radius); 
        return (linearVelocity, angularVelocity);
    }
}