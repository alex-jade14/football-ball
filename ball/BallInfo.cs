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
            GetMeasurement().GetMass(),
            GetMeasurement().GetCrossSectionalArea(),
            environment
        );
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
}