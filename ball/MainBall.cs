using Godot;
using System;
using System.Collections.Generic;

public partial class MainBall : BallBase
{
    public EventManager events;
    public MeshInstance3D mesh;
    public CollisionShape3D collisionNode;

    public override void _Ready(){
        SetGlobalPosition(new Vector3(GlobalPosition.X, GetMeasurement().GetRadius(), GlobalPosition.Z));
        mesh = (MeshInstance3D) GetNode("MeshInstance3D");
        mesh.SetScale(new Vector3(1,1,1) * GetMeasurement().GetRadius());
        collisionNode = (CollisionShape3D) GetNode("CollisionShape3D");
        SphereShape3D collisionShape = (SphereShape3D) collisionNode.GetShape();
        collisionShape.SetRadius(GetMeasurement().GetRadius());
        StandardMaterial3D firstMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(0);
        StandardMaterial3D secondMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(1);
        StandardMaterial3D thirdMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(2);
        firstMaterial.SetAlbedo(GetModel().GetFirstColor());
        secondMaterial.SetAlbedo(GetModel().GetSecondColor());
        thirdMaterial.SetAlbedo(GetModel().GetThirdColor());
        ApplyImpulse(new Vector3(-10, 5, 10), new Vector3(0, -GetMeasurement().GetRadius(), GetMeasurement().GetRadius()));
    }

    public override void _PhysicsProcess(double delta)
    {
        SimulatePhysics();
        ApplyAngularRotation();
        MoveAndSlide();
    }

    public void ApplyAngularRotation(){
        mesh.SetGlobalTransform(
            RigidBodyHelper.CalculateAngularRotation(
                GetAngularVelocity(),
                mesh.GetGlobalTransform()
            )
        );
    }
}