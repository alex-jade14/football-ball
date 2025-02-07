using Godot;

public partial class RotationalMotionHelper
{

    public static Vector3 CalculateTorque(Vector3 position, Vector3 force){
        return position.Cross(force);
    }
    public static Vector3 CalculateTorqueWithManyForces(Vector3 position, Vector3[] forces){
        Vector3 forcesAmount = new(0,0,0);
        if(forces.Length > 0){
            for(int i = 0; i < forces.Length; i++){
                forcesAmount += forces[i];
            }
        }
        return position.Cross(forcesAmount);
    }
}