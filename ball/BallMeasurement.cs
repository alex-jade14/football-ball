using Godot;
using System;
using System.ComponentModel;
using System.Data;

public partial class BallMeasurement
{
    private float _mass;
    private float _inertia;
    private float _radius;
    private float _diameter;
    private float _circumference;
    private float _crossSectionalArea;

    public BallMeasurement(float mass, float circumference){
        SetMassInGrams(mass);
        SetCircumferenceInCentemeters(circumference);
    }

    public float GetMass(){
        return _mass;
    }

    public void SetMass(float mass){
        _mass = mass;
        UpdateValuesWhenMassIsUpdated();
    }

    public float GetMassInGrams(){
        return UnitsConverterHelper.ConvertKilogramToGram(_mass);
    }

    public void SetMassInGrams(float mass){
        _mass = UnitsConverterHelper.ConvertGramToKilogram(mass);
    }

    public float GetInertia(){
        return _inertia;
    }

    public float GetRadius(){
        return _radius;
    }

    public void SetRadius(float radius){
        _radius = radius;
        UpdateLengthsWhenRadiusIsUpdated();
    }

    public float GetRadiusInCentimeters(){
        return UnitsConverterHelper.ConvertMetersToCentimeters(_radius);
    }

    public void SetRadiusInCentemeters(float radius){
        SetRadius(UnitsConverterHelper.ConvertCentimetersToMeters(radius));
    }

    public float GetDiameter(){
        return _diameter;
    }

    public void SetDiameter(float diameter){
        _diameter = diameter;
        UpdateLengthsWhenDiameterIsUpdated();
    }

    public float GetDiameterInCentimeters(){
        return UnitsConverterHelper.ConvertMetersToCentimeters(_diameter);
    }

    public void SetDiameterInCentemeters(float diameter){
        _diameter = UnitsConverterHelper.ConvertCentimetersToMeters(diameter);

    }

    public float GetCircumference(){
        return _circumference;
    }

    public void SetCircumference(float circumference){
        _circumference = circumference;
        UpdateLengthsWhenCircumferenceIsUpdated();
    }

    public float GetCircumferenceInCentimeters(){
        return UnitsConverterHelper.ConvertMetersToCentimeters(_circumference);
    }

    public void SetCircumferenceInCentemeters(float circumference){
        SetCircumference(UnitsConverterHelper.ConvertCentimetersToMeters(circumference));
    }

    public float GetCrossSectionalArea(){
        return _crossSectionalArea;
    }

    public float GetCrossSectionalAreaInCentimeters(){
        return UnitsConverterHelper.ConvertMetersToCentimeters(_crossSectionalArea);
    }

    public void SetCrossSectionalAreaInCentemeters(float crossSectionalArea){
        _crossSectionalArea = UnitsConverterHelper.ConvertCentimetersToMeters(crossSectionalArea);
    }

    private void CalculateInertia(){
        _inertia = RotationalMotionHelper.CalculateInertiaFromSphere(_mass, _radius);
    }

    private void CalculateDiameterFromRadius(){
        _diameter = SphereHelper.CalculateDiameterFromRadius(_radius);
    }

    private void CalculateCircumferenceFromRadius(){
        _circumference = SphereHelper.CalculateCircumferenceFromRadius(_radius);
    }

    private void CalculateCrossSectionalArea(){
        _crossSectionalArea = SphereHelper.CalculateCrossSectionalArea(_radius);
    }

    private void CalculateRadiusFromDiameter(){
        SetRadius(SphereHelper.CalculateRadiusFromDiameter(_diameter));
    }

    private void CalculateRadiusFromCircumference(){
        SetRadius(SphereHelper.CalculateRadiusFromCircumference(_circumference));
    }

    private void UpdateValuesWhenMassIsUpdated(){
        CalculateInertia();
    }

    private void UpdateLengthsWhenRadiusIsUpdated(){
        CalculateDiameterFromRadius();
        CalculateCircumferenceFromRadius();
        CalculateCrossSectionalArea();
        CalculateInertia();
    }

    private void UpdateLengthsWhenDiameterIsUpdated(){
        CalculateRadiusFromDiameter();
    }

    private void UpdateLengthsWhenCircumferenceIsUpdated(){
        CalculateRadiusFromCircumference();
    }
}
