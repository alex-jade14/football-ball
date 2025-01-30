using Godot;
using Godot.Collections;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class BallInfo
{
    private BallMeasurement _measurement;
    private BallPhysicsParameters _physicsParameters;

    public BallInfo(float mass, float circumference, float coefficientOfRestitution, float rotationalCoefficientOfRestitution,
    float frictionCoefficient, float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
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