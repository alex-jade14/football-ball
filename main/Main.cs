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
        Dictionary loadedData = GetContentFromJson();
        WorldEnvironment environment = GetWorldEnvironment(loadedData);
        MainBall mainBall = GetMainBall(loadedData, environment);
        PackedScene ballScene = (PackedScene) GD.Load("res://ball/shadow_ball.tscn");
		ShadowBall shadowBall = (ShadowBall) ballScene.Instantiate();
        shadowBall = (ShadowBall) mainBall.DeepCopy(shadowBall);
        AddChild(shadowBall);
        mainBall.events.Attach(shadowBall);
        mainBall.ApplyImpulse(
            new Vector3(-10, 6, 10),
            new Vector3(
                0, 
                -mainBall.GetInfo().GetMeasurement().GetRadius(), 
                mainBall.GetInfo().GetMeasurement().GetRadius()
            )
        );
    }

    public Dictionary GetContentFromJson(){
        String filePath = "info.JSON";
        String jsonAsText = File.ReadAllText(filePath);
        Json json = new Json();
        Error error = json.Parse(jsonAsText);
        if(error != Error.Ok){
            GD.Print(error);
        }
        return (Dictionary) json.Data;
    }

    public WorldEnvironment GetWorldEnvironment(Dictionary loadedData){
        Dictionary environmentData = (Dictionary) loadedData["environment"];
        WorldEnvironment environment = new(
            (float) environmentData["density_of_fluid"]
        );
        return environment;
    }

    public MainBall GetMainBall(Dictionary loadedData, WorldEnvironment environment){
        Dictionary ballInfo = (Dictionary) loadedData["ball"];
        Dictionary info = (Dictionary) ballInfo["info"];
        Dictionary model = (Dictionary) info["model"];
        Dictionary measurement = (Dictionary) info["measurement"];
        Dictionary physicsParameters = (Dictionary) info["physics_parameters"];
        PackedScene ballScene = (PackedScene) GD.Load("res://ball/main_ball.tscn");
        MainBall ball = (MainBall) ballScene.Instantiate();
        ball.create(
            (String) info["name"],
            (String) model["pattern"],
            ColorHelper.GetColor((Dictionary) model["firstColor"]),
            ColorHelper.GetColor((Dictionary) model["secondColor"]),
            ColorHelper.GetColor((Dictionary) model["thirdColor"]),
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
        return ball;
    }


}
