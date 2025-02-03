using Godot;
using Godot.NativeInterop;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public partial class BallBase : CharacterBody3D
{
    protected BallInfo _info;
    private Godot.Vector3 _linearVelocity;
    private Godot.Vector3 _angularVelocity;
    private bool _wasOnFloor;
    private bool _canResetBouncing;
    private bool _canRoll;
    private bool _canApplyAirResistance = true;
    private bool _canApplyMagnusEffect = true;
    private bool _detectedCollision;
    protected BallMeasurement _measurement;
    protected BallPhysicsParameters _physicsParameters;
    protected MeshInstance3D mesh;
    protected CollisionShape3D collisionNode;

    public void Create(float mass, float circumference, float coefficientOfRestitution, float rotationalCoefficientOfRestitution, float frictionCoefficient,
    float dragCoefficient, float liftCoefficient, float angularDampingCoefficient, WorldEnvironment environment){
        _info = new BallInfo(
            mass,
            circumference,
            coefficientOfRestitution,
            rotationalCoefficientOfRestitution,
            frictionCoefficient,
            dragCoefficient,
            liftCoefficient,
            angularDampingCoefficient,
            environment
        );
        _measurement = _info.GetMeasurement();
        _physicsParameters = _info.GetPhysicsParameters();
    }
    
    public override void _Ready(){
        ScaleMeshAndCollisionToRadius();
        PlaceBallOverFloor();
    }


    public BallInfo GetInfo(){
        return _info;
    }

    public void SetInfo(BallInfo info){
        _info = info;
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

    protected bool CanRoll(){
        return _canRoll;
    }

    protected void CanRoll(bool canRoll){
        _canRoll = canRoll;
    }
    
    public bool CanApplyAirResistance(){
        return _canApplyAirResistance;
    }

    public void CanApplyAirResistance(bool canApplyAirResistance){
        _canApplyAirResistance = canApplyAirResistance;
    }

    public bool CanApplyMagnusEffect(){
        return _canApplyMagnusEffect;
    }

    public void CanApplyMagnusEffect(bool canApplyMagnusEffect){
        _canApplyMagnusEffect = canApplyMagnusEffect;
    }

    public bool IsACollisionDetected(){
        return _detectedCollision;
    }

    public void IsACollisionDetected(bool detectedCollision){
        _detectedCollision = detectedCollision;
    }

    public BallMeasurement GetMeasurement(){
        return _measurement;
    }

    public BallPhysicsParameters GetPhysicsParameters(){
        return _physicsParameters;
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

    public virtual void ApplyImpulse(Godot.Vector3 impulse, Godot.Vector3 positionWhereImpulseIsApplied){
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
        CanRoll(false);
        IsACollisionDetected(GetSlideCollisionCount() > 0);
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
                if(!WasOnFloor() && !CanResetBouncing()){
                    CanRoll(false);
                    ApplyCoefficientOfRestitution(
                        collision.GetNormal()
                    );
                    WasOnFloor(true);
                    if(GetLinearVelocity().Y <= 0.5){
                        CanResetBouncing(true);
                        CanRoll(true);
                    }
                }
                else if(GetLinearVelocity().Y <= 0.5){
                    CanRoll(true);
                }
                ApplyFloorEffect(collision.GetPosition(), collision.GetNormal());
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
            GetAngularVelocity(),
            CanApplyAirResistance(),
            CanApplyMagnusEffect()
        );
        for(int i = 0; i < forcesFromAerodynamicEffects.Length; i++){
            forcesToApply.Add(forcesFromAerodynamicEffects[i]);
        }
        GetPhysicsParameters().SetDragForce(forcesFromAerodynamicEffects[0]);
        GetPhysicsParameters().SetMagnusEffectForce(forcesFromAerodynamicEffects[1]);
        Godot.Vector3 forceFromGravity = PhysicsHelper.GetForceFromGravity(GetMeasurement().GetMass());
        if(Mathf.Abs(GetLinearVelocity().Y) > 0){
            forcesToApply.Add(forceFromGravity);
        }
        ApplyCentralForces(forcesToApply.ToArray());
        Godot.Vector3 auxLinearVelocity = GetLinearVelocity();
        auxLinearVelocity.Y = AerodynamicHelper.limitToTerminalVelocity(
            GetPhysicsParameters().GetTerminalVelocity(),
            auxLinearVelocity.Y
        );
        SetLinearVelocity(auxLinearVelocity);
    }

    private void ApplyCoefficientOfRestitution(Godot.Vector3 collisionNormal){
        SetLinearVelocity(
            CollisionHelper.CalculateNewVelocityFromCoefficientOfRestitution(
                GetPhysicsParameters().GetCoefficientOfRestitution(),
                GetPhysicsParameters().GetRotationalCoefficientOfRestitution(),
                GetMeasurement().GetMass(),
                collisionNormal,
                GetLinearVelocity()
            )
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

    private void ApplyFloorEffect(Vector3 collisionPosition, Vector3 collisionNormal){
        Godot.Vector3 frictionForce = -GetPhysicsParameters().GetFrictionForce() * GetLinearVelocity().Normalized();
        ApplyRollingWithoutSlipping(frictionForce, ToLocal(collisionPosition), collisionNormal);
        Godot.Vector3 auxLinearVelocity = GetLinearVelocity();
        if (Mathf.Abs(GetLinearVelocity().X) < 0.1){
            auxLinearVelocity.X = 0;
        }
        if (Mathf.Abs(GetLinearVelocity().Z) < 0.1){
            auxLinearVelocity.Z = 0;
        }
        SetLinearVelocity(auxLinearVelocity);
    }

    private void ApplyRollingWithoutSlipping(Godot.Vector3 force, Godot.Vector3 collisionPosition, Godot.Vector3 collisionNormal){
        (_linearVelocity, _angularVelocity) = RollingDynamicsHelper.CalculateRollingWithoutSlipping(
            force, 
            GetUpDirection(), 
            GetMeasurement().GetMass(),
            GetLinearVelocity(), GetAngularVelocity(),
            CanRoll(),
            GetMeasurement().GetRadius(),
            collisionPosition,
            GetMeasurement().GetInertia(),
            GetPhysicsParameters().GetFrictionCoefficient(),
            collisionNormal,
            GetPhysicsParameters().GetFrictionForce()
        );
    }

    public void ScaleMeshAndCollisionToRadius(){
        mesh = (MeshInstance3D) GetNode("MeshInstance3D");
        mesh.SetScale(new Vector3(1,1,1) * GetMeasurement().GetRadius());
        collisionNode = (CollisionShape3D) GetNode("CollisionShape3D");
        SphereShape3D collisionShape = (SphereShape3D) collisionNode.GetShape();
        collisionShape.SetRadius(GetMeasurement().GetRadius());
    }

    private void PlaceBallOverFloor(){
        SetGlobalPosition(new Vector3(GlobalPosition.X, GetMeasurement().GetRadius(), GlobalPosition.Z));
    }
}