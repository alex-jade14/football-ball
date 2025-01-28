using Godot;
using System;

public partial class BallPhysicsParameters
{
    private float _coefficientOfRestitution;
    private float _frictionCoefficient;
    private float _dragCoefficient;
    private float _liftCoefficient;
    private float _terminalVelocity;
    private float _angularDampingCoefficient;
    private float _frictionForce;
    private float _normalForce;

    private WorldEnvironment _environment;

    public BallPhysicsParameters(float coefficientOfRestitution, float frictionCoefficient, float dragCoefficient,
    float liftCoefficient, float angularDampingCoefficient, float mass, WorldEnvironment environment){
        SetCoefficientOfRestitution(coefficientOfRestitution);
        SetFrictionCoefficient(frictionCoefficient);
        SetDragCoefficient(dragCoefficient);
        SetLiftCoefficient(liftCoefficient);
        SetAngularDampingCoefficient(angularDampingCoefficient);
        SetNormalForce(mass);
        SetEnvironment(environment);
    }

    public float GetCoefficientOfRestitution(){
        return _coefficientOfRestitution;
    }

    public void SetCoefficientOfRestitution(float coefficientOfRestitution){
        _coefficientOfRestitution = coefficientOfRestitution;
        UpdateValuesWhenFrictionIsUpdated();
    }

    public float GetFrictionCoefficient(){
        return _frictionCoefficient;
    }

    public void SetFrictionCoefficient(float frictionCoefficient){
        _frictionCoefficient = frictionCoefficient;
        UpdateValuesWhenFrictionIsUpdated();
    }

    public float GetDragCoefficient(){
        return _dragCoefficient;
    }

    public void SetDragCoefficient(float dragCoefficient){
        _dragCoefficient = dragCoefficient;
    }

    public float GetLiftCoefficient(){
        return _liftCoefficient;
    }

    public void SetLiftCoefficient(float liftCoefficient){
        _liftCoefficient = liftCoefficient;
    }

    public float GetTerminalVelocity(){
        return _terminalVelocity;
    }

    public float GetAngularDampingCoefficient(){
        return _angularDampingCoefficient;
    }

    public void SetAngularDampingCoefficient(float angularDampingCoefficient){
        _angularDampingCoefficient = angularDampingCoefficient;
    }

    public float GetFrictionForce(){
        return _frictionForce;
    }

    public float GetNormalForce(){
        return _normalForce;
    }

    public void SetNormalForce(float mass){  
        _normalForce = NewtonsFirstLawHelper.CalculateNormalForce(mass);
        UpdateValuesWhenNormalForceIsUpdated();
    }

    public WorldEnvironment GetEnvironment(){
        return _environment;
    }

    public void SetEnvironment(WorldEnvironment environment){
        _environment = environment;
    }

    private void UpdateValuesWhenFrictionIsUpdated(){
        CalculateFrictionForce(_frictionCoefficient, _normalForce);
    }

    private void UpdateValuesWhenNormalForceIsUpdated(){
        CalculateFrictionForce(_frictionCoefficient, _normalForce);
    }

    private void CalculateFrictionForce(float frictionCoefficient, float normalForce){
        _frictionForce = FrictionHelper.CalculateFrictionForce(frictionCoefficient, normalForce);
    }

    private void CalculateTerminalVelocity(float crossSectionalArea, float mass, float dragCoefficient, float densityOfFluid){
        AerodynamicHelper.CalculateTerminalVelocity(crossSectionalArea, mass, dragCoefficient, densityOfFluid);
    }
}