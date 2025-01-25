using Godot;
using System;

public partial class BallBase : CharacterBody3D, IPrototype
{
    private BallInfo _info { get; set; }
    private Godot.Vector3 _linearVelocity {
        get{
            return GetLinearVelocity();
        }
        set{
            SetLinearVelocity(value);
        }
    }
    private Godot.Vector3 _angularVelocity {
        get{
            return GetAngularVelocity();
        }
        set{
            SetAngularVelocity(value);
        }
    }
    private bool _wasOnFloor{
        get{
            return WasOnFloor();
        }
        set{
            WasOnFloor(value);
        }
    }
     private bool _canResetBouncing{
        get{
            return CanResetBouncing();
        }
        set{
            CanResetBouncing(value);
        }
    }
    private BallMeasurement _measurement {
        get{
            return GetMeasurement();
        }
    }
    private BallPhysicsParameters _physicsParameters {
        get{
            return GetPhysicsParameters();
        }
    }

    public Godot.Vector3 GetLinearVelocity(){
        return _linearVelocity;
    }

    public void SetLinearVelocity(Godot.Vector3 linearVelocity){
        _linearVelocity = linearVelocity;
    }
    
    public Godot.Vector3 GetLinearVelocityInKMH(){
        return new Godot.Vector3(
            UnitsConverterHelper.ConvertMSToKMH(_linearVelocity.X),
            UnitsConverterHelper.ConvertMSToKMH(_linearVelocity.Y),
            UnitsConverterHelper.ConvertMSToKMH(_linearVelocity.Z)
        );
    }

    public void SetLinearVelocityInKMH(Godot.Vector3 linearVelocity){
        _linearVelocity = new Godot.Vector3(
            UnitsConverterHelper.ConvertKMHToMS(_linearVelocity.X),
            UnitsConverterHelper.ConvertKMHToMS(_linearVelocity.Y),
            UnitsConverterHelper.ConvertKMHToMS(_linearVelocity.Z)
        );
    }

    public Godot.Vector3 GetAngularVelocity(){
        return _angularVelocity;
    }

    public void SetAngularVelocity(Godot.Vector3 angularVelocity){
        _angularVelocity = angularVelocity;
    }

    public Godot.Vector3 GetAngularVelocityInKMH(){
        return new Godot.Vector3(
            UnitsConverterHelper.ConvertMSToKMH(_angularVelocity.X),
            UnitsConverterHelper.ConvertMSToKMH(_angularVelocity.Y),
            UnitsConverterHelper.ConvertMSToKMH(_angularVelocity.Z)
        );
    }

    public void SetAngularVelocityInKMH(Godot.Vector3 linearVelocity){
        _angularVelocity = new Godot.Vector3(
            UnitsConverterHelper.ConvertKMHToMS(_angularVelocity.X),
            UnitsConverterHelper.ConvertKMHToMS(_angularVelocity.Y),
            UnitsConverterHelper.ConvertKMHToMS(_angularVelocity.Z)
        );
    }

    public bool WasOnFloor(){
        return _wasOnFloor;
    }

    public void WasOnFloor(bool wasOnFloor){
        _wasOnFloor = wasOnFloor;
    }
    public bool CanResetBouncing(){
        return _canResetBouncing;
    }

    public void CanResetBouncing(bool canResetBouncing){
        _canResetBouncing = canResetBouncing;
    }


    public BallMeasurement GetMeasurement(){
        return _measurement;
    }

    public BallPhysicsParameters GetPhysicsParameters(){
        return _physicsParameters;
    }


    public GodotObject ShallowCopy(){
        return (BallBase) this.MemberwiseClone();
    }

    public GodotObject DeepCopy(){
        BallBase clone = (BallBase) this.MemberwiseClone();
        return clone;
    }

    public void SimulatePhysics(){
        CollisionResponse();
        AirConditions();
        AvoidCrossingTheFloor();
        ApplyLinearVelocity();
        ApplyAngularVelocity();
    }

    private void CollisionResponse(){
        for(int i = 0; i < GetSlideCollisionCount(); i++){
            KinematicCollision3D collision = GetSlideCollision(i);
            GodotObject collider = collision.GetCollider();
            if (
                !collision.GetNormal().IsEqualApprox(UpDirection) && 
                GetGlobalPosition().Y > GetMeasurement().GetRadius()
            ){
                // Ball collides with an object that is not the floor
                SetLinearVelocity(
                    CollisionHelper.BounceVelocity(
                        GetLinearVelocity(),
                        collider.HasMethod("GetCoefficientOfRestitution") ? (float) collider.Get("coefficientOfRestitution") : 0.8f,
                        collision.GetNormal()
                    )
                );
            }
            else if (IsOnFloor()){
                // Ball collides with the floor
                ApplyFloorEffect(collision.GetPosition());
            }
        }
    }

    private void AirConditions(){
        if(!IsOnFloor()){
            ApplyAirEffects();
            WasOnFloor(false);
        }
        else if(CanResetBouncing()){
            ResetBouncing();
        }
    }

    public void ApplyAirEffects(){
        Godot.Vector3[] forcesToApply = Array.Empty<Godot.Vector3>();
        //Godot.Vector3[] forcesFromAerodynamicEffects = 
        AerodynamicHelper.AerodynamicEffectsOfASphereOnAir(
            GetMeasurement().GetCrossSectionalArea(),
            GetPhysicsParameters().GetDragCoefficient(),
            GetPhysicsParameters().GetLiftCoefficient(),
            GetLinearVelocity(),
            GetAngularVelocity()
        );
    }

    private void ResetBouncing(){
        Godot.Vector3 auxLinearVelocity = GetLinearVelocity();
        auxLinearVelocity.Y = 0;
        SetLinearVelocity(auxLinearVelocity);
        WasOnFloor(true);
        CanResetBouncing(false);
    }

    private void AvoidCrossingTheFloor(){
        Godot.Vector3 auxGlobalPosition = GetGlobalPosition();
        if(auxGlobalPosition.Y < GetMeasurement().GetRadius()){
            auxGlobalPosition.Y = GetMeasurement().GetRadius();
        }
        SetGlobalPosition(auxGlobalPosition);
    }

    private void ApplyLinearVelocity(){
        Godot.Vector3 auxLinearVelocity = GetLinearVelocity();
        if (Velocity.Y == -(auxLinearVelocity.Y)){
            auxLinearVelocity.Y = -auxLinearVelocity.Y;
        }
        SetVelocity(auxLinearVelocity);
    }

    private void ApplyAngularVelocity(){
        SetAngularVelocity(
            RigidBodyHelper.CalculateAngularDamping(
                GetPhysicsParameters().GetAngularDampingCoefficient(),
                GetAngularVelocity()
            )
        );
    }

    private void ApplyFloorEffect(Vector3 collisionPosition){
        Godot.Vector3 frictionForce = -GetPhysicsParameters().GetFrictionForce() * GetLinearVelocity().Normalized();
        ApplyRollingWithoutSlipping(frictionForce, collisionPosition);
        Godot.Vector3 auxLinearVelocity = GetLinearVelocity();
        if (Mathf.Abs(GetLinearVelocity().X) < 0.1){
            auxLinearVelocity.X = 0;
        }
        if (Mathf.Abs(GetLinearVelocity().Z) < 0.1){
            auxLinearVelocity.Z = 0;
        }
        SetLinearVelocity(auxLinearVelocity);
    }

    private void ApplyRollingWithoutSlipping(Godot.Vector3 force, Godot.Vector3 collisionPosition){
        (_linearVelocity, _angularVelocity) = RollingDynamicsHelper.CalculateRollingWithoutSlipping(
            force, collisionPosition, GetMeasurement().GetMass(), GetMeasurement().GetCircumference(),
            GetLinearVelocity(), GetAngularVelocity()
        );
    }
}