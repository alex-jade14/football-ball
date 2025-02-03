using Godot;
using Godot.Collections;
using System;

public partial class DrawHelper
{   
    public static void drawPoint(Vector3 position, float radius, Color color, float persist_ms = 0){
        MeshInstance3D meshInstance = new MeshInstance3D();
        SphereMesh sphereMesh = new SphereMesh();
        OrmMaterial3D material = new OrmMaterial3D();

        meshInstance.SetMesh(sphereMesh);
        meshInstance.CastShadow = GeometryInstance3D.ShadowCastingSetting;
        meshInstance.SetPosition(position);

        sphereMesh.SetRadius(radius);
        sphereMesh.SetHeight(radius*2);
        sphereMesh.SetMaterial(material);

        material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
        material.albedo_color = color

        return await final_cleanup(mesh_instance, persist_ms)
    }
	
}