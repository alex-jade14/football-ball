using Godot;
using System;
using System.Collections.Generic;

public partial class MainBall : BallBase
{
    public EventManager events;

    public override void _Ready(){
        SetGlobalPosition(new Vector3(GlobalPosition.X, GetMeasurement().GetRadius(), GlobalPosition.Z));
        SetScale(new Vector3(1,1,1) * GetMeasurement().GetRadius());
        MeshInstance3D mesh = (MeshInstance3D) GetNode("MeshInstance3D");
        StandardMaterial3D firstMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(0);
        StandardMaterial3D secondMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(1);
        StandardMaterial3D thirdMaterial = (StandardMaterial3D) mesh.GetActiveMaterial(2);
        firstMaterial.SetAlbedo(GetModel().GetFirstColor());
        secondMaterial.SetAlbedo(GetModel().GetSecondColor());
        thirdMaterial.SetAlbedo(GetModel().GetThirdColor());
    }
}