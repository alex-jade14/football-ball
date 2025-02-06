using System;
using System.Collections.Generic;
using Godot;

public partial class Drawer : Node3D
{
    public List<MeshInstance3D> meshInstances = new List<MeshInstance3D>();
    public int meshInstancesCounter;
    public StandardMaterial3D material = new();

    public Drawer(){
        material.ShadingMode = StandardMaterial3D.ShadingModeEnum.Unshaded;
        CreateNewMeshInstance();
    }

    public void ClearMeshInstances(){
        if(meshInstances.Count > 0){
            for(int i = 0; i < meshInstances.Count; i++){
                String nodeName = meshInstances[i].GetName();
                RemoveChild(GetNode(nodeName));
            }
            meshInstances.Clear();
            CreateNewMeshInstance();
        }
    }

    public void Line(Vector3 pos1, Vector3 pos2, Color? color = null)
    {
        ImmediateMesh currentMesh = (ImmediateMesh) meshInstances[meshInstancesCounter].Mesh;
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
        meshInstances.Add(new MeshInstance3D());
        meshInstancesCounter = meshInstances.Count - 1;
        meshInstances[meshInstancesCounter].Mesh = new ImmediateMesh();
        AddChild(meshInstances[meshInstancesCounter]);
    }
	
}