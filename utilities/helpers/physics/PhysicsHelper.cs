using Godot;

public partial class PhysicsHelper{
    public static float Gravity = (float) ProjectSettings.GetSetting("physics/3d/default_gravity");

    public static Vector3 GetVelocityFromGravity(){
        return new Vector3(0, -Gravity, 0) * PhysicsServerHelper.DeltaFromPhysicsProcess;
    } 
}