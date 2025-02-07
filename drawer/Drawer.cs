using System;
using System.Collections.Generic;
using Godot;

public partial class Drawer : Node3D
{
    public List<MeshInstance3D> MeshInstances = new();
    public int MeshInstancesCounter;
    public StandardMaterial3D material = new();

    public Drawer(){
        material.ShadingMode = StandardMaterial3D.ShadingModeEnum.Unshaded;
        CreateNewMeshInstance();
    }

    public void ClearMeshInstances(){
        if(MeshInstances.Count > 0){
            for(int i = 0; i < MeshInstances.Count; i++){
                String nodeName = MeshInstances[i].GetName();
                RemoveChild(GetNode(nodeName));
            }
            MeshInstances.Clear();
            CreateNewMeshInstance();
        }
    }

    public void Line(Vector3 pos1, Vector3 pos2, Color? color = null)
    {
        ImmediateMesh currentMesh = (ImmediateMesh) MeshInstances[MeshInstancesCounter].Mesh;
        if(currentMesh.GetSurfaceCount() == 255){
            CreateNewMeshInstance();
        }
        material.AlbedoColor = color ?? Colors.WhiteSmoke;
        currentMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, material);
        currentMesh.SurfaceAddVertex(pos1);
        currentMesh.SurfaceAddVertex(pos2);
        currentMesh.SurfaceEnd();
    }

    public void CreateNewMeshInstance(){
        MeshInstances.Add(new MeshInstance3D());
        MeshInstancesCounter = MeshInstances.Count - 1;
        MeshInstances[MeshInstancesCounter].Mesh = new ImmediateMesh();
        AddChild(MeshInstances[MeshInstancesCounter]);
    }
	
}