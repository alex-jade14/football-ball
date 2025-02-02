using Godot;
using System;

public partial class SphereHelper : CircleHelper{
    public static float CalculateInertiaFromSphere(float mass, float radius){
        return 0.66f * mass * Mathf.Pow(radius, 2f);
    }
}