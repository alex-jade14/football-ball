using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class Main : Node3D
{
    public override void _Ready(){
        WorldEnvironment environment = GetWorldEnvironment();
        MainBall mainBall = GetMainBall(environment);
		ShadowBall shadowBall = GetShadowBall(mainBall);
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
        PackedScene mainBallScene = (PackedScene) GD.Load("res://ball/main_ball.tscn");
        MainBall mainBall = (MainBall) mainBallScene.Instantiate();
        mainBall.Create(
            (String) model["pattern"],
            ColorHelper.GetColor((Dictionary) model["firstColor"]),
            ColorHelper.GetColor((Dictionary) model["secondColor"]),
            ColorHelper.GetColor((Dictionary) model["thirdColor"]),
            (float) measurement["mass"],
            (float) measurement["circumference"],
            (float) physicsParameters["coefficientOfRestitution"],
            (float) physicsParameters["rotationalCoefficientOfRestitution"],
            (float) physicsParameters["frictionCoefficient"],
            (float) physicsParameters["dragCoefficient"],
            (float) physicsParameters["liftCoefficient"],
            (float) physicsParameters["angularDampingCoefficient"],
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

    public Debug GetDebugScreen(MainBall mainBall, ShadowBall shadowBall){
        PackedScene debugScene = (PackedScene) GD.Load("res://debug/Debug.tscn");
        Debug debug = (Debug) debugScene.Instantiate();
        debug.Create(mainBall, shadowBall);
        AddChild(debug);
        return debug;
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
                            },
                            {
                                "thirdColor", new Dictionary{
                                    {"r", 35},
                                    {"g", 35},
                                    {"b", 35},
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
                            {"rotationalCoefficientOfRestitution", 0.6f},
                            {"frictionCoefficient", 0.62f},
                            {"dragCoefficient", 0.47f},
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
            {"densityOfFluid", 1.225f}
        };
    }

    // public Dictionary GetInitialDebugData(MainBall mainBall, ShadowBall shadowBall){
	// 	return new Dictionary{
	// 		{
	// 			"interaction", new Dictionary{
	// 				{"initialPosition", mainBall.GetGlobalPosition()},
	// 				{"initialImpulse", new Vector3(-10, 6, 10)},
	// 				{"positionWhereImpulseIsApplied", new Vector3(
    //                         0,
    //                         -mainBall.GetMeasurement().GetRadius(),
    //                         mainBall.GetMeasurement().GetRadius()
    //                     )
    //                 },
	// 				{"impulseFactor", 2}
	// 			}
	// 		},
	// 		{
	// 			"parameters", new Dictionary{
	// 				{
	// 					"measurement", new Dictionary{
	// 						{"mass", mainBall.GetMeasurement().GetMass()},
	// 						{"circumference", mainBall.GetMeasurement().GetCircumference()},
	// 					}
	// 				},
	// 				{
	// 					"physicsParameters", new Dictionary{
	// 						{"coefficientOfRestitution", mainBall.GetPhysicsParameters().GetCoefficientOfRestitution()},
    //                         {"rotationalCoefficientOfRestitution", mainBall.GetPhysicsParameters().GetCoefficientOfRestitution()},
    //                         {"frictionCoefficient", mainBall.GetPhysicsParameters().GetFrictionCoefficient()},
    //                         {"dragCoefficient", mainBall.GetPhysicsParameters().GetDragCoefficient()},
    //                         {"liftCoefficient", mainBall.GetPhysicsParameters().GetLiftCoefficient()},
    //                         {"angularDampingCoefficient", mainBall.GetPhysicsParameters().GetAngularDampingCoefficient()}
	// 					}
	// 				}
	// 			}
	// 		},
    //         {
    //             "additional", new Dictionary{
    //                 {
    //                     "physics", new Dictionary{
    //                         {"airResistance", true},
    //                         {"magnusEffect", true}
    //                     }
    //                 },
    //                 {
    //                     "environment", new Dictionary{
    //                         {"airDensity", mainBall.GetPhysicsParameters().GetEnvironment().GetDensityOfFluid()},
    //                         {"rotationReductionFactor", 4},
    //                         {"interpolationCoefficient", 0.1}
    //                     }
    //                 },
    //                 {
    //                     "shadowBall", new Dictionary{
    //                         {"shadowBall", false}
    //                     }
    //                 }
    //             }
    //         },
    //         {
    //             "model", new Dictionary{
    //                 "pattern", 
    //             }
    //         }
	// 	};
	// }


}
