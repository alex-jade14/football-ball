using Godot;
using System.Collections.Generic;

public partial class BallBase : CharacterBody3D
{
    protected BallInfo Info;
    protected BallMeasurement Measurement;
    protected BallPhysicsParameters PhysicsParameters;
    protected CollisionShape3D CollisionNode;
    protected bool CanDetectCollisions;
    private Vector3 _linearVelocity;
    private Vector3 _angularVelocity;
    private bool _wasOnFloor;
    private bool _canResetBouncing;
    private bool _isBouncing;
    private bool _canRoll;
    private bool _detectedCollision;
    private bool _canApplyAirResistance;
    private bool _canApplyMagnusEffect;

    public void Create(float mass, float circumference, float coefficientOfRestitution, float frictionCoefficient,
    float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
        Info = new BallInfo(
            mass,
            circumference,
            coefficientOfRestitution,
            frictionCoefficient,
            dragCoefficient,
            liftCoefficient,
            angularDampingCoefficient,
            environment
        );
        _canApplyAirResistance = true;
        _canApplyMagnusEffect = true;
        Measurement = Info.GetMeasurement();
        PhysicsParameters = Info.GetPhysicsParameters();
    }

    public override void _Ready(){
        ScaleCollisionToRadius();
        PlaceBallOverFloor();
    }

    public BallInfo GetInfo(){
        return Info;
    }

    public BallMeasurement GetMeasurement(){
        return Measurement;
    }

    public BallPhysicsParameters GetPhysicsParameters(){
        return PhysicsParameters;
    }

    public Vector3 GetLinearVelocity(){
        return _linearVelocity;
    }

    public Vector3 GetLinearVelocityInKMH(){
        return new Vector3(
            UnitsConverterHelper.ConvertMSToKMH(_linearVelocity.X),
            UnitsConverterHelper.ConvertMSToKMH(_linearVelocity.Y),
            UnitsConverterHelper.ConvertMSToKMH(_linearVelocity.Z)
        );
    }

    public Vector3 GetAngularVelocity(){
        return _angularVelocity;
    }

    protected bool WasOnFloor(){
        return _wasOnFloor;
    }

    protected bool CanResetBouncing(){
        return _canResetBouncing;
    }

    protected bool IsBouncing(){
        return _isBouncing;
    }

    protected bool CanRoll(){
        return _canRoll;
    }

    public bool IsACollisionDetected(){
        return _detectedCollision;
    }

    public bool CanApplyAirResistance(){
        return _canApplyAirResistance;
    }

    public bool CanApplyMagnusEffect(){
        return _canApplyMagnusEffect;
    }

    public void SetInfo(BallInfo info){
        Info = info;
    }

    public void SetLinearVelocity(Vector3 linearVelocity){
        _linearVelocity = linearVelocity;
    }

    public void SetLinearVelocityInKMH(Vector3 linearVelocity){
        _linearVelocity = new Vector3(
            UnitsConverterHelper.ConvertKMHToMS(linearVelocity.X),
            UnitsConverterHelper.ConvertKMHToMS(linearVelocity.Y),
            UnitsConverterHelper.ConvertKMHToMS(linearVelocity.Z)
        );
    }

    public void SetAngularVelocity(Vector3 angularVelocity){
        _angularVelocity = angularVelocity;
    }

    protected void WasOnFloor(bool wasOnFloor){
        _wasOnFloor = wasOnFloor;
    }

    protected void CanResetBouncing(bool canResetBouncing){
        _canResetBouncing = canResetBouncing;
    }

    protected void IsBouncing(bool isBouncing){
        _isBouncing = isBouncing;
    }

    protected void CanRoll(bool canRoll){
        _canRoll = canRoll;
    }

    public void IsACollisionDetected(bool detectedCollision){
        _detectedCollision = detectedCollision;
    }

    public void CanApplyAirResistance(bool canApplyAirResistance){
        _canApplyAirResistance = canApplyAirResistance;
    }

    public void CanApplyMagnusEffect(bool canApplyMagnusEffect){
        _canApplyMagnusEffect = canApplyMagnusEffect;
    }

    public void ApplyCentralForces(Vector3[] forcesToApply){
        _linearVelocity = RigidBodyHelper.CalculateCentralForces(
            forcesToApply,
            GetMeasurement().GetMass(),
            _linearVelocity
        );
    }

    public void ApplyForces(Vector3[] forcesToApply, Vector3 positionWhereForcesAreApplied){
        _linearVelocity =  RigidBodyHelper.CalculateForces(
            forcesToApply,
            positionWhereForcesAreApplied,
            GetMeasurement().GetMass(),
            GetMeasurement().GetInertia(),
            _linearVelocity
        );
    }

    public void ApplyCentralImpulse(Vector3 impulse){
        _linearVelocity = RigidBodyHelper.CalculateCentralImpulse(
            impulse,
            GetMeasurement().GetMass(),
            _linearVelocity
        );
    }

    public virtual void ApplyImpulse(Vector3 impulse, Vector3 positionWhereImpulseIsApplied){
        (_linearVelocity, _angularVelocity) = RigidBodyHelper.CalculateImpulse(
            impulse,
            positionWhereImpulseIsApplied,
            GetMeasurement().GetMass(),
            GetMeasurement().GetInertia(),
            _linearVelocity,
            _angularVelocity
        );
    }

    protected void SimulatePhysics(){
        CollisionResponse();
        AirConditions();
        AvoidCrossingTheFloor();
        ApplyLinearVelocity();
        ApplyAngularVelocity();
    }

    public virtual void CollisionResponse(){
        if(CanDetectCollisions){
            _detectedCollision = GetSlideCollisionCount() > 0;
            _isBouncing = false;
            for(int i = 0; i < GetSlideCollisionCount(); i++){
                KinematicCollision3D collision = GetSlideCollision(i);
                if (!collision.GetNormal().IsEqualApprox(UpDirection) &&
                    GetGlobalPosition().Y > GetMeasurement().GetRadius()){
                    // Ball collides with an object that is not the floor
                    // This is not implemented yet
                }
                else if (IsOnFloor()){
                    // Ball collides with the floor
                    if(!_wasOnFloor && !_canResetBouncing){
                        ApplyCoefficientOfRestitution(
                            collision.GetNormal()
                        );
                        if(GetLinearVelocity().Y > 0.5f){
                            _wasOnFloor = true;
                            _isBouncing = true;
                        }
                        else{
                            _canResetBouncing = true;
                        }
                    }
                    ApplyFloorEffect(collision);
                    break;
                }
            }
        }
    }

    private void AirConditions(){
        if(!IsOnFloor()){
            ApplyAirEffects();
            _wasOnFloor = false;
        }
        else if(_canResetBouncing){
            ResetBouncing();
        }
    }

    private void ApplyAirEffects(){
        List<Vector3> forcesToApply = new();
        AddForcesToApplyFromAerodynamicEffects(forcesToApply);
        AddForceToApplyFromGravity(forcesToApply);
        ApplyCentralForces(forcesToApply.ToArray());
        LimitToTerminalVelocity();
    }

    private void AddForcesToApplyFromAerodynamicEffects(List<Vector3> forcesToApply){
        Vector3[] forcesFromAerodynamicEffects = AerodynamicHelper.AerodynamicEffectsOfASphereOnAir(
            GetPhysicsParameters().GetDragCoefficient(),
            GetPhysicsParameters().GetEnvironment().GetDensityOfFluid(),
            GetPhysicsParameters().GetLiftCoefficient(),
            GetMeasurement().GetCrossSectionalArea(),
            _linearVelocity,
            _angularVelocity,
            _canApplyAirResistance,
            _canApplyMagnusEffect
        );
        for(int i = 0; i < forcesFromAerodynamicEffects.Length; i++){
            forcesToApply.Add(forcesFromAerodynamicEffects[i]);
        }
        GetPhysicsParameters().SetDragForce(forcesFromAerodynamicEffects[0]);
        GetPhysicsParameters().SetMagnusEffectForce(forcesFromAerodynamicEffects[1]);
    }

    private void AddForceToApplyFromGravity(List<Vector3> forcesToApply){
        Vector3 forceFromGravity = PhysicsHelper.GetForceFromGravity(GetMeasurement().GetMass());
        if(Mathf.Abs(GetLinearVelocity().Y) > 0){
            forcesToApply.Add(forceFromGravity);
        }
    }

    private void LimitToTerminalVelocity(){
        _linearVelocity.Y = AerodynamicHelper.LimitToTerminalVelocity(
            GetPhysicsParameters().GetTerminalVelocity(),
            _linearVelocity.Y
        );
    }

    private void ApplyCoefficientOfRestitution(Vector3 collisionNormal){
        _linearVelocity = CollisionHelper.CalculateNewVelocityFromCoefficientOfRestitution(
            GetPhysicsParameters().GetCoefficientOfRestitution(),
            collisionNormal,
            GetLinearVelocity()
        );
    }

    private void ResetBouncing(){
        _linearVelocity.Y = 0;
        _wasOnFloor = true;
        _canResetBouncing = false;
        _isBouncing = false;
    }

    private void AvoidCrossingTheFloor(){
        if(GetGlobalPosition().Y < GetMeasurement().GetRadius()){
            SetGlobalPosition(new Vector3(GetGlobalPosition().X, GetMeasurement().GetRadius(), GetGlobalPosition().Z));
        }
    }

    private void ApplyLinearVelocity(){
        if (Velocity.Y == -_linearVelocity.Y){
            _linearVelocity.Y = -_linearVelocity.Y;
        }
        SetVelocity(_linearVelocity);
    }

    private void ApplyAngularVelocity(){
        _angularVelocity = RigidBodyHelper.CalculateAngularDamping(
            GetPhysicsParameters().GetAngularDampingCoefficient(),
            _angularVelocity
        );
    }

    private void ApplyFloorEffect(KinematicCollision3D collision){
        DecreaseVelocityFromFriction(collision.GetPosition(), collision.GetNormal());
        if (Mathf.Abs(_linearVelocity.X) < 0.1){
            _linearVelocity.X = 0;
        }
        if (Mathf.Abs(_linearVelocity.Z) < 0.1){
            _linearVelocity.Z = 0;
        }
    }

    private void DecreaseVelocityFromFriction(Vector3 collisionPosition, Vector3 collisionNormal){
        Vector3 frictionForce = -GetPhysicsParameters().GetFrictionForce() * GetLinearVelocity().Normalized();
        Vector3 previousLinearVelocity = _linearVelocity;
        _linearVelocity = FrictionHelper.CalculateLinearVelocityFromFriction(_linearVelocity, frictionForce, GetMeasurement().GetMass());
        if(_isBouncing){
            ApplyFrictionToAngularVelocityWhenBouncing(previousLinearVelocity, ToLocal(collisionPosition), collisionNormal);
            _canRoll = false;
        }
        else{
            if(_linearVelocity.Length() - (_linearVelocity.Length() * GetMeasurement().GetRadius()) < 0.01f){
                _angularVelocity = _angularVelocity.Lerp(ApplyFrictionToAngularVelocityWhenRolling(), 0.25f);
                _canRoll = true;
            }
            else if(_canRoll && _linearVelocity.Y == 0){
                _angularVelocity = ApplyFrictionToAngularVelocityWhenRolling();
            }
        }
    }

    private void ApplyFrictionToAngularVelocityWhenBouncing(Vector3 previousLinearVelocity, Vector3 collisionPosition, Vector3 collisionNormal){
        _angularVelocity = CollisionHelper.CalculateNewAngularVelocityFromFrictionWhenBouncing(
            _linearVelocity,
            previousLinearVelocity,
            _angularVelocity,
            collisionPosition,
            collisionNormal,
            GetMeasurement().GetRadius(),
            GetMeasurement().GetMass(),
            GetMeasurement().GetInertia(),
            GetPhysicsParameters().GetFrictionCoefficient()
        );
    }

    private Vector3 ApplyFrictionToAngularVelocityWhenRolling(){
        return FrictionHelper.CalculateAngularVelocityFromFriction(
            _linearVelocity,
            GetUpDirection(),
            GetMeasurement().GetRadius()
        );
    }

    public void ScaleCollisionToRadius(){
        CollisionNode = (CollisionShape3D) GetNode("CollisionShape3D");
        SphereShape3D collisionShape = (SphereShape3D) CollisionNode.GetShape();
        collisionShape.SetRadius(GetMeasurement().GetRadius());
    }

    private void PlaceBallOverFloor(){
        SetGlobalPosition(new Vector3(GetGlobalPosition().X, GetMeasurement().GetRadius(), GetGlobalPosition().Z));
    }
}