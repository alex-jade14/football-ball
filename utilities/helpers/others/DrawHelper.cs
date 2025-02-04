using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class DrawHelper
{
    public static MeshInstance3D Line(Vector3 pos1, Vector3 pos2, Color? color = null)
    {
        var meshInstance = new MeshInstance3D();
        var immediateMesh = new ImmediateMesh();
        var material = new StandardMaterial3D();

        meshInstance.Mesh = immediateMesh;
        meshInstance.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;

        immediateMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, material);
        immediateMesh.SurfaceAddVertex(pos1);
        immediateMesh.SurfaceAddVertex(pos2);
        immediateMesh.SurfaceEnd();

        material.ShadingMode = StandardMaterial3D.ShadingModeEnum.Unshaded;
        material.AlbedoColor = color ?? Colors.WhiteSmoke;

        (Engine.GetMainLoop() as SceneTree).Root.AddChild(meshInstance);

        return meshInstance;
    }
	
}