using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class Main : Node3D
{
    public override void _Ready(){
        Dictionary loadedData = GetContentFromJson();
        WorldEnvironment environment = GetWorldEnvironment(loadedData);
        MainBall mainBall = GetMainBall(loadedData, environment);
		ShadowBall shadowBall = GetShadowBall(mainBall);
        SubscribeShadowBallToEvents(mainBall, shadowBall);
        // mainBall.ApplyImpulse(
        //     new Vector3(-10, 6, 10),
        //     new Vector3(
        //         0, 
        //         -mainBall.GetInfo().GetMeasurement().GetRadius(), 
        //         mainBall.GetInfo().GetMeasurement().GetRadius()
        //     )
        // );
        
        RenderDebugScreens(GetDebugSCreens(loadedData));
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
        PackedScene mainBallScene = (PackedScene) GD.Load("res://ball/main_ball.tscn");
        MainBall mainBall = (MainBall) mainBallScene.Instantiate();
        mainBall.create(
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
        AddChild(mainBall);
        return mainBall;
    }
    public ShadowBall GetShadowBall(MainBall mainBall){
        PackedScene shadowBallScene = (PackedScene) GD.Load("res://ball/shadow_ball.tscn");
        ShadowBall shadowBall = (ShadowBall) shadowBallScene.Instantiate();
        shadowBall = (ShadowBall) mainBall.DeepCopy(shadowBall);
        AddChild(shadowBall);
        return shadowBall;
    }

    public void SubscribeShadowBallToEvents(MainBall mainBall, ShadowBall shadowBall){
        mainBall.events.Attach(shadowBall);
    }

    public void RenderDebugScreens(Godot.Collections.Array screens){
        DebugInfo source = new DebugComponent();
        source = new DebugDecorator(source);
        PackedScene settingsScene = (PackedScene) GD.Load("res://settings/settings.tscn");
        Settings settings = (Settings) settingsScene.Instantiate();
        AddChild(settings);
    }

    public Godot.Collections.Array GetDebugSCreens(Dictionary loadedData){
        Dictionary debugInfo = (Dictionary) loadedData["debug"];
        var debugScreens = debugInfo["screens"];
        return debugScreens.AsGodotArray();
    }
}
