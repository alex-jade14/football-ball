using Godot;
using System;

public partial class FrictionHelper
{

    public static float CalculateFrictionForce(float frictionCoefficient, float normalForce){
        return frictionCoefficient * normalForce;
    }
}