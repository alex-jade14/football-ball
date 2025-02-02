using Godot;
using System;

public partial class RotationalMotionHelper
{

    public static Godot.Vector3 CalculateTorque(Godot.Vector3 position, Godot.Vector3 force){
        return position.Cross(force);
    }
    public static Godot.Vector3 CalculateTorqueWithManyForces(Godot.Vector3 position, Godot.Vector3[] forces){
        Godot.Vector3 forcesAmount = new Godot.Vector3(0,0,0);
        if(forces.Length > 0){
            for(int i = 0; i < forces.Length; i++){
                forcesAmount += forces[i];
            }
        }
        return position.Cross(forcesAmount);
    }
}