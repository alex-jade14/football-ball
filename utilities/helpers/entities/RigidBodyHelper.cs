using Godot;
using Godot.NativeInterop;
using System;

public partial class RigidBodyHelper
{

    public static Godot.Vector3 CalculateCentralForces(Godot.Vector3[] forcesToApply, float mass, Godot.Vector3 linearVelocity){
        Godot.Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForcesInVectorForm(
            forcesToApply,
            mass
        );
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        return linearVelocity;
    }

    public static Godot.Vector3 CalculateForces(Godot.Vector3[] forcesToApply, Godot.Vector3 positionWhereForcesAreApplied, float mass, float inertia,
    Godot.Vector3 linearVelocity){
        Godot.Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForcesInVectorForm(forcesToApply, mass);
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(linearAcceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        return linearVelocity;
    }

    public static Godot.Vector3 CalculateCentralImpulse(Godot.Vector3 impulse, float mass, Godot.Vector3 linearVelocity){
        Godot.Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(impulse, mass);
        linearVelocity += linearAcceleration;
        return linearVelocity;
    }

    public static (Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity) CalculateImpulse(Godot.Vector3 impulse, 
    Godot.Vector3 positionWhereImpulseIsApplied, float mass, float inertia, Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity){
        Godot.Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(impulse, mass);
        linearVelocity += linearAcceleration;
        //positionWhereImpulseIsApplied * 2 -> 2 value is a magic number
        Godot.Vector3 torque = RotationalMotionHelper.CalculateTorque(positionWhereImpulseIsApplied * 2, impulse);
        Godot.Vector3 angularAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(torque, inertia);
        angularVelocity += MotionHelper.CalculateVelocityFromAcceleration(angularAcceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        return (linearVelocity, angularVelocity);
    }

    public static Godot.Vector3 CalculateAngularDamping(float angularDampingCoefficient, Godot.Vector3 angularVelocity){
        float angularResult = 1.0f - PhysicsServerHelper.deltaFromPhysicsProcess * angularDampingCoefficient;
        if (angularResult < 0){
            angularResult = 0;
        }
        angularVelocity *= angularResult;
        return angularVelocity;
    }

    public static Transform3D CalculateAngularRotation(Godot.Vector3 angularVelocity, Transform3D meshToRotateTransform){
        float angularVelocityMagnitude = angularVelocity.Length();
        if(angularVelocityMagnitude > 0){
            Godot.Vector3 angularVelocityAxis = angularVelocity / angularVelocityMagnitude;
            Basis rotation = new Basis(angularVelocityAxis, angularVelocityMagnitude * PhysicsServerHelper.deltaFromPhysicsProcess);
            meshToRotateTransform.Basis *= rotation;
            meshToRotateTransform.Orthonormalized();
        }
        return meshToRotateTransform;
    }
}