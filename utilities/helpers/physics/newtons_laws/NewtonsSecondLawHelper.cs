using Godot;

public partial class NewtonsSecondLawHelper
{
    public static float CalculateForceWithAcceleration(float mass, float acceleration){
        return mass * acceleration;
    }

    public static float CalculateAccelerationWithForce(float force, float mass){
        return force / mass;
    }

    public static Vector3 CalculateAccelerationWithForceInVectorForm(Vector3 force, float mass){
        Vector3 acceleration = force / mass;
        return acceleration;
    }
    
    public static Vector3 CalculateAccelerationWithForcesInVectorForm(Vector3[] forces, float mass){
        Vector3 acceleration = new (0,0,0);
        if(forces.Length > 0){
            for(int i = 0; i < forces.Length; i++){
                acceleration += forces[i];
            }
            acceleration /= mass;
        }

        return acceleration;
    }
}