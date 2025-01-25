using Godot;
using System;

public partial class NewtonsFirstLawHelper : PhysicsHelper
{
    public static float CalculateNormalForce(float mass){
        return mass * gravity;
    }
}