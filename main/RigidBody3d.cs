using Godot;
using System;

public partial class RigidBody3d : RigidBody3D
{

    public Camera3D camera;
    public override void _Ready(){
        ApplyImpulse(new Vector3(10,6,-10), new Vector3(0, -0.11f, 0.11f) * 10);
        camera = (Camera3D) GetParent().GetNode("Camera3D2");
    }

    public override void _PhysicsProcess(double delta)
    {
        Godot.Vector3 ballPosition = GetGlobalPosition();
        camera.SetGlobalPosition(camera.GetGlobalPosition().Lerp(new Vector3(ballPosition.X, ballPosition.Y + 1.61f, ballPosition.Z - 3.609f), 0.9f));
    }
}
