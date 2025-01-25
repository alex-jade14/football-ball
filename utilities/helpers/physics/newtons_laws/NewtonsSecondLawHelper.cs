using Godot;
using System;

public partial class NewtonsSecondLawHelper
{
    public static float CaulcateForceWithAcceleration(float mass, float acceleration){
        return mass * acceleration;
    }

    public static CalculateAccelerationWithForce(float force, float mass){
        return force / mass;
    }
}