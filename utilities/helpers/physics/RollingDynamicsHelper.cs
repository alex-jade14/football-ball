using Godot;
using System;

public partial class RollingDynamicsHelper{
    public static Vector3 CalculateAngularVelocityFromRollingWithoutSlipping(Vector3 linearVelocity,
    Vector3 upDirection, float radius){
        Vector3 angularVelocity = FrictionHelper.CalculateAngularVelocityFromFriction(linearVelocity, upDirection, radius); 
        return angularVelocity;
    }
}