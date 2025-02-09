using Godot;

public partial class MotionHelper
{
    public static Vector3 CalculateVelocityFromAcceleration(Vector3 acceleration){
        return acceleration * PhysicsServerHelper.DeltaFromPhysicsProcess;
    }

}