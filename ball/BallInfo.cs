public partial class BallInfo
{
    private BallMeasurement _measurement;
    private BallPhysicsParameters _physicsParameters;

    public BallInfo(float mass, float circumference, float coefficientOfRestitution, float frictionCoefficient,
    float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
        _measurement = new BallMeasurement(
            mass,
            circumference
        );
        _physicsParameters = new BallPhysicsParameters(
            coefficientOfRestitution,
            frictionCoefficient,
            dragCoefficient,
            liftCoefficient,
            angularDampingCoefficient,
            environment
        );
        UpdateNormalForce();
        UpdateTerminalVelocity();
    }

    public BallMeasurement GetMeasurement(){
        return _measurement;
    }

    public BallPhysicsParameters GetPhysicsParameters(){
        return _physicsParameters;
    }

    public void SetMeasurement(BallMeasurement measurement){
        _measurement = measurement;
    }

    public void SetPhysicsParameters(BallPhysicsParameters physicsParameters){
        _physicsParameters = physicsParameters;
    }
    
    public void UpdateNormalForce(){
        _physicsParameters.SetNormalForce(_measurement.GetMass());
    }

    public void UpdateTerminalVelocity(){
        _physicsParameters.SetTerminalVelocity(_measurement.GetCrossSectionalArea(), _measurement.GetMass());
    }
}