using Godot;

public partial class PhysicsHelper{
    public static float Gravity = (float) ProjectSettings.GetSetting("physics/3d/default_gravity");

    public static Vector3 GetForceFromGravity(float mass){
        float forceFromGravity = NewtonsSecondLawHelper.CalculateForceWithAcceleration(mass, Gravity);
        Vector3 forceFromGravityInVectorForm = forceFromGravity * new Vector3(0,-1f,0);
        return forceFromGravityInVectorForm;
    } 
}