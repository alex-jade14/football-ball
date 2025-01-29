using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Array = System.Array;

public partial class Main : Node3D
{
    public override void _Ready(){
       
        String filePath = "info.JSON";
        String jsonAsText = File.ReadAllText(filePath);
        Json json = new Json();
        Error error = json.Parse(jsonAsText);
        if(error != Error.Ok){
            GD.Print(error);
        }

        Dictionary loadedData = (Dictionary) json.Data;
        Dictionary environmentData = (Dictionary) loadedData["environment"];
        WorldEnvironment environment = new(
            (float) environmentData["density_of_fluid"]
        );
        Dictionary ballInfo = (Dictionary) loadedData["ball"];
        Dictionary info = (Dictionary) ballInfo["info"];
        Dictionary modelData = (Dictionary) info["model"];
        Dictionary firstColor = (Dictionary) modelData["firstColor"];
        Dictionary secondColor = (Dictionary) modelData["secondColor"];
        Dictionary thirdColor = (Dictionary) modelData["thirdColor"];
        Dictionary measurement = (Dictionary) info["measurement"];
        Dictionary physicsParameters = (Dictionary) info["physics_parameters"];
        PackedScene ballScene = (PackedScene) GD.Load("res://ball/ball.tscn");
        MainBall ball = (MainBall) ballScene.Instantiate();
        ball.create(
            (String) info["name"],
            (String) modelData["pattern"],
            new Color((float) firstColor["r"] / 255, (float) firstColor["g"] / 255, (float) firstColor["b"] / 255, (float) firstColor["a"] / 255),
            new Color((float) secondColor["r"] / 255, (float) secondColor["g"] / 255, (float) secondColor["b"] / 255, (float) firstColor["a"] / 255),
            new Color((float) thirdColor["r"] / 255, (float) thirdColor["g"] / 255, (float) thirdColor["b"] / 255, (float) firstColor["a"] / 255),
            (float) measurement["mass"],
            (float) measurement["circumference"],
            (float) physicsParameters["coefficient_of_restitution"],
            (float) physicsParameters["rotational_coefficient_of_restitution"],
            (float) physicsParameters["friction_coefficient"],
            (float) physicsParameters["drag_coefficient"],
            (float) physicsParameters["lift_coefficient"],
            (float) physicsParameters["angular_damping_coefficient"],
            environment
        );
        AddChild(ball);

    }
}
