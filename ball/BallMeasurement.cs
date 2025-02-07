using Godot;

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

    public float GetMassInGrams(){
        return UnitsConverterHelper.ConvertKilogramToGram(_mass);
    }

    public float GetInertia(){
        return _inertia;
    }
    
    public float GetRadius(){
        return _radius;
    }

    public float GetRadiusInCentimeters(){
        return UnitsConverterHelper.ConvertMetersToCentimeters(_radius);
    }

    public float GetDiameter(){
        return _diameter;
    }

    public float GetDiameterInCentimeters(){
        return UnitsConverterHelper.ConvertMetersToCentimeters(_diameter);
    }

    public float GetCircumference(){
        return _circumference;
    }

    public float GetCircumferenceInCentimeters(){
        return UnitsConverterHelper.ConvertMetersToCentimeters(_circumference);
    }

    public float GetCrossSectionalArea(){
        return _crossSectionalArea;
    }

    public void SetMass(float mass){
        _mass = Mathf.Clamp(mass, BallParametersRanges.MinMassValue, BallParametersRanges.MaxMassValue);
        UpdateValuesWhenMassIsUpdated();
    }

    public void SetMassInGrams(float mass){
        SetMass(UnitsConverterHelper.ConvertGramToKilogram(mass));
    }

    public void SetRadius(float radius){
        _radius = Mathf.Clamp(radius, BallParametersRanges.MinRadiusValue, BallParametersRanges.MaxRadiusValue);
        UpdateLengthsWhenRadiusIsUpdated();
    }

    public void SetRadiusInCentemeters(float radius){
        SetRadius(UnitsConverterHelper.ConvertCentimetersToMeters(radius));
    }

    public void SetDiameter(float diameter){
        _diameter = Mathf.Clamp(
            diameter,
            BallParametersRanges.MinDiameterValue,
            BallParametersRanges.MaxDiameterValue
        );
        UpdateLengthsWhenDiameterIsUpdated();
    }

    public void SetDiameterInCentemeters(float diameter){
        SetDiameter(UnitsConverterHelper.ConvertCentimetersToMeters(diameter));
    }

    public void SetCircumference(float circumference){
        _circumference =  Mathf.Clamp(
            circumference,
            BallParametersRanges.MinCircumferenceValue,
            BallParametersRanges.MaxCircumferenceValue
        );
        UpdateLengthsWhenCircumferenceIsUpdated();
    }

    public void SetCircumferenceInCentemeters(float circumference){
        SetCircumference(UnitsConverterHelper.ConvertCentimetersToMeters(circumference));
    }

    private void CalculateInertia(){
        _inertia = SphereHelper.CalculateInertiaFromThinSphere(_mass, _radius);
    }

    private void CalculateDiameterFromRadius(){
        _diameter = CircleHelper.CalculateDiameterFromRadius(_radius);
    }

    private void CalculateCircumferenceFromRadius(){
        _circumference = CircleHelper.CalculateCircumferenceFromRadius(_radius);
    }

    private void CalculateCrossSectionalArea(){
        _crossSectionalArea = CircleHelper.CalculateCrossSectionalArea(_radius);
    }

    private void CalculateRadiusFromDiameter(){
        SetRadius(CircleHelper.CalculateRadiusFromDiameter(_diameter));
    }

    private void CalculateRadiusFromCircumference(){
        SetRadius(CircleHelper.CalculateRadiusFromCircumference(_circumference));
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
