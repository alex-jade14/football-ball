using Godot;

public partial class MotionHelper
{
    public static Vector3 CalculateVelocityFromAcceleration(Vector3 acceleration){
        return acceleration * PhysicsServerHelper.DeltaFromPhysicsProcess;
    }

    // Reference: https://www.youtube.com/watch?v=7JSwHggGhVI
    public static Vector3 CalculateVelocityFromImpulse(Vector3 impulse, float mass){
        if(mass <= 0){
            return new Vector3(0,0,0);
        }
        return impulse / mass;
    }

}