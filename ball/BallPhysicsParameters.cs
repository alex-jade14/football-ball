using Godot;
using System;

public partial class BallPhysicsParameters
{
    private float _coefficientOfRestitution { get; set; }
    private float _frictionCoefficient {
        get{
            return GetFrictionCoefficient();
        }
        set{
            SetFrictionCoefficient(value);
        }
    }
    private float _dragCoefficient {
        get{
            return GetDragCoefficient();
        }
        set {
            SetDragCoefficient(value);
        }
    }
    private float _liftCoefficient {
        get{
            return GetLiftCoefficient();
        }
        set {
            SetLiftCoefficient(value);
        }
    }
    private float _terminalVelocity { get; set; }
    private float _angularDampingCoefficient {
        get{
            return GetAngularDampingCoefficient();
        }
    }
    private float _frictionForce {
        get{
            return GetFrictionForce();
        }
        set {}
    }
    private float _normalForce {
        get {
            return GetNormalForce();
        }
        set {
            SetNormalForce(value);
        }
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

    public float GetAngularDampingCoefficient(){
        return _angularDampingCoefficient;
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

    private void UpdateValuesWhenFrictionIsUpdated(){
        CalculateFrictionForce(_frictionCoefficient, _normalForce);
    }

    private void UpdateValuesWhenNormalForceIsUpdated(){
        CalculateFrictionForce(_frictionCoefficient, _normalForce);
    }

    private void CalculateFrictionForce(float frictionCoefficient, float normalForce){
        _frictionForce = FrictionHelper.CalculateFrictionForce(frictionCoefficient, normalForce);
    }

}