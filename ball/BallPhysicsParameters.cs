using Godot;

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
    private Vector3 _dragForce;
    private Vector3 _magnusEffectForce;

    public BallPhysicsParameters(float coefficientOfRestitution, float frictionCoefficient, float dragCoefficient, float liftCoefficient,
    float angularDampingCoefficient, WorldEnvironment environment){
        SetCoefficientOfRestitution(coefficientOfRestitution);
        SetFrictionCoefficient(frictionCoefficient);
        SetDragCoefficient(dragCoefficient);
        SetLiftCoefficient(liftCoefficient);
        SetAngularDampingCoefficient(angularDampingCoefficient);
        SetEnvironment(environment);
    }

    public float GetCoefficientOfRestitution(){
        return _coefficientOfRestitution;
    }

    public float GetFrictionCoefficient(){
        return _frictionCoefficient;
    }

    public float GetDragCoefficient(){
        return _dragCoefficient;
    }

    public float GetLiftCoefficient(){
        return _liftCoefficient;
    }

    public float GetTerminalVelocity(){
        return _terminalVelocity;
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

    public WorldEnvironment GetEnvironment(){
        return _environment;
    }

    public Vector3 GetDragForce(){
        return _dragForce;
    }

    public Vector3 GetMagnusEffectForce(){
        return _magnusEffectForce;
    }

    public void SetCoefficientOfRestitution(float coefficientOfRestitution){
        _coefficientOfRestitution = Mathf.Clamp(
            coefficientOfRestitution,
            BallParametersRanges.MinCoefficientOfRestitutionValue,
            BallParametersRanges.MaxCoefficientOfRestitutionValue
        );
    }

    public void SetFrictionCoefficient(float frictionCoefficient){
        _frictionCoefficient = Mathf.Clamp(
            frictionCoefficient,
            BallParametersRanges.MinFrictionCoefficientValue,
            BallParametersRanges.MaxFrictionCoefficientValue
        );
        UpdateValuesWhenFrictionCoefficientIsUpdated();
    }

    public void SetDragCoefficient(float dragCoefficient){
        _dragCoefficient = Mathf.Clamp(
            dragCoefficient,
            BallParametersRanges.MinDragCoefficientValue,
            BallParametersRanges.MaxDragCoefficientValue
        );
    }

    public void SetLiftCoefficient(float liftCoefficient){
        _liftCoefficient = Mathf.Clamp(
            liftCoefficient,
            BallParametersRanges.MinLiftCoefficientValue,
            BallParametersRanges.MaxLiftCoefficientValue
        );
    }

    public void SetTerminalVelocity(float crossSectionalArea, float mass){
        CalculateTerminalVelocity(crossSectionalArea, mass, _dragCoefficient, _environment.GetDensityOfFluid());
    }

    public void SetAngularDampingCoefficient(float angularDampingCoefficient){
        _angularDampingCoefficient = Mathf.Clamp(
            angularDampingCoefficient,
            BallParametersRanges.MinAngularDampingCoefficientValue,
            BallParametersRanges.MaxAngularDampingCoefficientValue
        );
    }

    public void SetNormalForce(float mass){
        _normalForce = NewtonsFirstLawHelper.CalculateNormalForce(mass);
        UpdateValuesWhenNormalForceIsUpdated();
    }

    public void SetEnvironment(WorldEnvironment environment){
        _environment = environment;
    }

    public void SetDragForce(Vector3 dragForce){
        _dragForce = dragForce;
    }

    public void SetMagnusEffectForce(Vector3 magnusEffectForce){
        _magnusEffectForce = magnusEffectForce;
    }

    private void CalculateFrictionForce(float frictionCoefficient, float normalForce){
        _frictionForce = FrictionHelper.CalculateFrictionForce(frictionCoefficient, normalForce);
    }

    private void CalculateTerminalVelocity(float crossSectionalArea, float mass, float dragCoefficient, float densityOfFluid){
        _terminalVelocity = AerodynamicHelper.CalculateTerminalVelocity(crossSectionalArea, mass, dragCoefficient, densityOfFluid);
    }

    private void UpdateValuesWhenFrictionCoefficientIsUpdated(){
        CalculateFrictionForce(_frictionCoefficient, _normalForce);
    }

    private void UpdateValuesWhenNormalForceIsUpdated(){
        CalculateFrictionForce(_frictionCoefficient, _normalForce);
    }
}