using Godot;
using System;
using System.ComponentModel;
using System.Data;

public partial class BallMeasurement
{
    private float _mass {
        get{
            return GetMass();
        }
        set{
            SetMass(value);
        }
    }
    private float _inertia {
        get{
             return GetInertia();
        }
        set{ }
    }
    private float _radius { 
        get{
            return GetRadius();
        }
        set{
            SetRadius(value);
        } 
    }
    private float _diameter {
        get{
            return GetDiameter();
        }
        set{
            SetDiameter(value);
        }
    }
    private float _circumference {
        get{
            return GetCircumference();
        }
        set{
            SetCircumference(value);
        }
    }
    private float _crossSectionalArea {
        get{
            return GetCrossSectionalArea();
        }
        set { }
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

    public void SetInertia(float inertia){
        _inertia = inertia;
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
