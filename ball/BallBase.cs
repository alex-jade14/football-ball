using Godot;
using Godot.NativeInterop;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public partial class BallBase : CharacterBody3D, IPrototype
{
    private BallInfo _info;
    private ShadowBall _shadowBall;
    private Godot.Vector3 _linearVelocity;
    private Godot.Vector3 _angularVelocity;
    private bool _wasOnFloor;
    private bool _canResetBouncing;
    protected BallModel _model;
    protected BallMeasurement _measurement;
    protected BallPhysicsParameters _physicsParameters;

    public void create(String name, String pattern, Color firstColor, Color secondColor, Color thirdColor,
    float mass, float circumference, float coefficientOfRestitution, float frictionCoefficient,
    float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
        _info = new BallInfo(
            name,
            pattern,
            firstColor,
            secondColor,
            thirdColor,
            mass,
            circumference,
            coefficientOfRestitution,
            frictionCoefficient,
            dragCoefficient,
            liftCoefficient,
            angularDampingCoefficient,
            environment
        );
        _shadowBall = new();
        _model = _info.GetModel();
        _measurement = _info.GetMeasurement();
        _physicsParameters = _info.GetPhysicsParameters();
    }

    public BallInfo GetInfo(){
        return _info;
    }

    public void SetInfo(BallInfo info){
        _info = info;
    }

    public ShadowBall GetShadowBall(){
        return _shadowBall;
    }

    public void SetShadowBall(ShadowBall shadowBall){
        _shadowBall = shadowBall;
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

    protected bool WasOnFloor(){
        return _wasOnFloor;
    }

    protected void WasOnFloor(bool wasOnFloor){
        _wasOnFloor = wasOnFloor;
    }
    protected bool CanResetBouncing(){
        return _canResetBouncing;
    }

    protected void CanResetBouncing(bool canResetBouncing){
        _canResetBouncing = canResetBouncing;
    }

    protected BallModel GetModel(){
        return _model;
    }

    protected BallMeasurement GetMeasurement(){
        return _measurement;
    }

    protected BallPhysicsParameters GetPhysicsParameters(){
        return _physicsParameters;
    }


    public GodotObject ShallowCopy(){
        return (BallBase) this.MemberwiseClone();
    }

    public GodotObject DeepCopy(){
        BallBase clone = (BallBase) this.MemberwiseClone();
        return clone;
    }

    public void ApplyCentralForces(Godot.Vector3[] forcesToApply){
        SetLinearVelocity(
            RigidBodyHelper.CalculateCentralForces(
                forcesToApply,
                GetMeasurement().GetMass(),
                GetLinearVelocity()
            )
        );
    }

    public void ApplyForces(Godot.Vector3[] forcesToApply, Godot.Vector3 positionWhereForcesAreApplied){
        SetLinearVelocity(
            RigidBodyHelper.CalculateForces(
                forcesToApply,
                positionWhereForcesAreApplied,
                GetMeasurement().GetMass(),
                GetMeasurement().GetInertia(),
                GetLinearVelocity()
            )
        );
    }

    public void ApplyCentralImpulse(Godot.Vector3 impulse){
        SetLinearVelocity(
            RigidBodyHelper.CalculateCentralImpulse(
                impulse, 
                GetMeasurement().GetMass(),
                GetLinearVelocity()
            )
        );
    }

    public void ApplyImpulse(Godot.Vector3 impulse, Godot.Vector3 positionWhereImpulseIsApplied){
        (_linearVelocity, _angularVelocity) = RigidBodyHelper.CalculateImpulse(
            impulse,
            positionWhereImpulseIsApplied,
            GetMeasurement().GetMass(),
            GetMeasurement().GetInertia(),
            GetLinearVelocity(),
            GetAngularVelocity()
        );
    }

    protected void SimulatePhysics(){
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
                if(!WasOnFloor() && !CanResetBouncing()){
                    ApplyCoefficientOfRestitution(
                        GetLinearVelocity(),
                        collision
                    );
                    WasOnFloor(true);
                    if(GetLinearVelocity().Y <= 0.4){
                        CanResetBouncing(true);
                    }
                    GD.Print("Hola");
                }
                
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

    private void ApplyAirEffects(){
        List<Godot.Vector3> forcesToApply = new List<Godot.Vector3>();
        Godot.Vector3[] forcesFromAerodynamicEffects = AerodynamicHelper.AerodynamicEffectsOfASphereOnAir(
            GetPhysicsParameters().GetDragCoefficient(),
            GetPhysicsParameters().GetEnvironment().GetDensityOfFluid(),
            GetPhysicsParameters().GetLiftCoefficient(),
            GetMeasurement().GetCrossSectionalArea(),
            GetLinearVelocity(),
            GetAngularVelocity()
        );
        for(int i = 0; i < forcesFromAerodynamicEffects.Length; i++){
            forcesToApply.Add(forcesFromAerodynamicEffects[i]);
        }
        Godot.Vector3 forceFromGravity = PhysicsHelper.GetForceFromGravity(GetMeasurement().GetMass());
        forcesToApply.Add(forceFromGravity);
        ApplyCentralForces(forcesToApply.ToArray());
        Godot.Vector3 auxLinearVelocity = GetLinearVelocity();
        auxLinearVelocity.Y = AerodynamicHelper.limitToTerminalVelocity(
            GetPhysicsParameters().GetTerminalVelocity(),
            auxLinearVelocity.Y
        );
        SetLinearVelocity(auxLinearVelocity);
    }

    private void ApplyCoefficientOfRestitution(Godot.Vector3 linearVelocity, KinematicCollision3D collision){
        Godot.Vector3 impulse = CollisionHelper.CalculateImpulseFromCoefficientOfRestitution(
            GetPhysicsParameters().GetCoefficientOfRestitution(),
            GetMeasurement().GetMass(),
            linearVelocity
        );
        ApplyImpulse(impulse, ToLocal(collision.GetPosition()));
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
        ApplyRollingWithoutSlipping(frictionForce, ToLocal(collisionPosition));
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