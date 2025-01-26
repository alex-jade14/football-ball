using Godot;
using System;

public partial class PhysicsHelper{
    public static float gravity = (float) ProjectSettings.GetSetting("physics/3d/default_gravity");

    public static Godot.Vector3 GetForceFromGravity(float mass){
        float forceFromGravity = NewtonsSecondLawHelper.CalculateForceWithAcceleration(mass, gravity);
        Godot.Vector3 forceFromGravityInVectorForm = forceFromGravity * new Godot.Vector3(0,-1f,0);
        return forceFromGravityInVectorForm;
    } 
}