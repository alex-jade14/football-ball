using Godot;
using Godot.Collections;
using System;

public partial class Main : Node3D
{
    public override void _Ready(){
        WorldEnvironment environment = GetWorldEnvironment();
        MainBall mainBall = GetMainBall(environment);
        Drawer drawer = GetDrawer();
		ShadowBall shadowBall = GetShadowBall(mainBall, drawer);
        SubscribeShadowBallToEvents(mainBall, shadowBall);
        Debug debug = GetDebugScreen(mainBall, shadowBall);
        debug.SetInitialDebugData();
        
    }

    public WorldEnvironment GetWorldEnvironment(){
        Dictionary environmentData = GetEnvironmentData();
        WorldEnvironment environment = new(
            (float) environmentData["densityOfFluid"]
        );
        return environment;
    }

    public MainBall GetMainBall(WorldEnvironment environment){
        Dictionary ballData = GetBallData();
        Dictionary info = (Dictionary) ballData["info"];
        Dictionary model = (Dictionary) info["model"];
        Dictionary measurement = (Dictionary) info["measurement"];
        Dictionary physicsParameters = (Dictionary) info["physicsParameters"];
        PackedScene mainBallScene = (PackedScene) GD.Load("res://ball/main_ball/main_ball.tscn");
        MainBall mainBall = (MainBall) mainBallScene.Instantiate();
        mainBall.Create(
            (String) model["pattern"],
            ColorHelper.GetColor((Dictionary) model["firstColor"]),
            ColorHelper.GetColor((Dictionary) model["secondColor"]),
            (float) measurement["mass"],
            (float) measurement["circumference"],
            (float) physicsParameters["coefficientOfRestitution"],
            (float) physicsParameters["frictionCoefficient"],
            (float) physicsParameters["dragCoefficient"],
            (float) physicsParameters["liftCoefficient"],
            (float) physicsParameters["angularDampingCoefficient"],
            environment
        );
        AddChild(mainBall);
        return mainBall;
    }
    public ShadowBall GetShadowBall(MainBall mainBall, Drawer drawer){
        PackedScene shadowBallScene = (PackedScene) GD.Load("res://ball/shadow_ball/shadow_ball.tscn");
        ShadowBall shadowBall = (ShadowBall) shadowBallScene.Instantiate();
        shadowBall = (ShadowBall) mainBall.DeepCopy(shadowBall);
        AddChild(shadowBall);
        shadowBall.SetDrawer(drawer);
        return shadowBall;
    }

    public void SubscribeShadowBallToEvents(MainBall mainBall, ShadowBall shadowBall){
        mainBall.Events.Attach("impulse", shadowBall);
        mainBall.Events.Attach("detectedCollision", shadowBall);
        mainBall.Events.Attach("updateMarker", shadowBall);
    }

    public Debug GetDebugScreen(MainBall mainBall, ShadowBall shadowBall){
        PackedScene debugScene = (PackedScene) GD.Load("res://debug/debug.tscn");
        Debug debug = (Debug) debugScene.Instantiate();
        debug.Create(mainBall, shadowBall);
        AddChild(debug);
        return debug;
    }

    public Drawer GetDrawer(){
        PackedScene drawerScene = (PackedScene) GD.Load("res://drawer/drawer.tscn");
        Drawer drawer = (Drawer) drawerScene.Instantiate();
        AddChild(drawer);
        return drawer;
    }

    public Dictionary GetBallData(){
        return new Dictionary{
            {
                "info", new Dictionary {
                    {"name", "Official Ball"},
                    {
                        "model", new Dictionary{
                            {"pattern", "hexagon-pentagon"},
                            {
                                "firstColor", new Dictionary{
                                    {"r", 231},
                                    {"g", 89},
                                    {"b", 39},
                                    {"a", 255}
                                }
                            },
                            {
                                "secondColor", new Dictionary{
                                    {"r", 231},
                                    {"g", 231},
                                    {"b", 231},
                                    {"a", 255}
                                }
                            }
                        }
                    },
                    {
                        "measurement", new Dictionary{
                            {"mass", 450},
                            {"circumference", 69.12}
                        }
                    },
                    {
                        "physicsParameters", new Dictionary{
                            {"coefficientOfRestitution", 0.67f},
                            {"frictionCoefficient", 0.62f}, // Reference: https://www.mdpi.com/2504-3900/49/1/92
                            {"dragCoefficient", 0.47f}, // https://es.wikipedia.org/wiki/Coeficiente_de_resistencia
                            {"liftCoefficient", 0.25f},
                            {"angularDampingCoefficient", 0.25f}
                        }
                    }
                }
            }
        };
    }

    public Dictionary GetEnvironmentData(){
        return new Dictionary{
            {"densityOfFluid", 1.225f} // Reference: https://es.wikipedia.org/wiki/Densidad_del_aire
        };
    }
}
