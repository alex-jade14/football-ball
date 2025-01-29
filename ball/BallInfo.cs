using Godot;
using Godot.Collections;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class BallInfo
{
    private String _name;
    private BallModel _model;
    private BallMeasurement _measurement;
    private BallPhysicsParameters _physicsParameters;

    public BallInfo(String name, String pattern, Color firstColor, Color secondColor, Color thirdColor,
    float mass, float circumference, float coefficientOfRestitution, float rotationalCoefficientOfRestitution,
    float frictionCoefficient, float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
        _name = name;
        _model = new BallModel(
            pattern,
            firstColor,
            secondColor,
            thirdColor
        );
        _measurement = new BallMeasurement(
            mass,
            circumference
        );
        _physicsParameters = new BallPhysicsParameters(
            coefficientOfRestitution,
            rotationalCoefficientOfRestitution,
            frictionCoefficient,
            dragCoefficient,
            liftCoefficient,
            angularDampingCoefficient,
            GetMeasurement().GetMass(),
            GetMeasurement().GetCrossSectionalArea(),
            environment
        );
    }

    public String GetName(){
        return _name;
    }

    public void SetName(String name){
        _name = name;
    }

    public BallModel GetModel(){
        return _model;
    }

    public void SetModel(BallModel model){
        _model = model;
    }

    public void SetName(BallModel model){
        _model = model;
    }

    public BallMeasurement GetMeasurement(){
        return _measurement;
    }

    public void SetMeasurement(BallMeasurement measurement){
        _measurement = measurement;
    }

    public BallPhysicsParameters GetPhysicsParameters(){
        return _physicsParameters;
    }

    public void SetPhysicsParameters(BallPhysicsParameters physicsParameters){
        _physicsParameters = physicsParameters;
    }

}