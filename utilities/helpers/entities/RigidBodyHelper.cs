using Godot;
using Godot.NativeInterop;
using System;

public partial class RigidBodyHelper
{

    public static Vector3 CalculateCentralForces(Vector3[] forcesToApply, float mass, Vector3 linearVelocity){
        Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForcesInVectorForm(
            forcesToApply,
            mass
        );
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        return linearVelocity;
    }

    public static Vector3 CalculateForces(Vector3[] forcesToApply, Vector3 positionWhereForcesAreApplied, float mass, float inertia,
    Vector3 linearVelocity){
        Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForcesInVectorForm(forcesToApply, mass);
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(linearAcceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        return linearVelocity;
    }

    public static Vector3 CalculateCentralImpulse(Vector3 impulse, float mass, Vector3 linearVelocity){
        Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(impulse, mass);
        linearVelocity += linearAcceleration;
        return linearVelocity;
    }

    public static (Vector3 linearVelocity, Vector3 angularVelocity) CalculateImpulse(Vector3 impulse, 
    Vector3 positionWhereImpulseIsApplied, float mass, float inertia, Vector3 linearVelocity, Vector3 angularVelocity){
        Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(impulse, mass);
        linearVelocity += linearAcceleration;
        Vector3 torque = RotationalMotionHelper.CalculateTorque(positionWhereImpulseIsApplied, impulse);
        Vector3 angularAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(torque, inertia);
        angularVelocity += MotionHelper.CalculateVelocityFromAcceleration(angularAcceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        return (linearVelocity, angularVelocity);
    }

    public static Vector3 CalculateAngularDamping(float angularDampingCoefficient, Vector3 angularVelocity){
        float angularResult = 1.0f - PhysicsServerHelper.deltaFromPhysicsProcess * angularDampingCoefficient;
        if (angularResult < 0){
            angularResult = 0;
        }
        angularVelocity *= angularResult;
        return angularVelocity;
    }

    public static Transform3D CalculateAngularRotation(Vector3 angularVelocity, Transform3D meshToRotateTransform){
        float angularVelocityMagnitude = angularVelocity.Length();
        if(angularVelocityMagnitude > 0){
            Vector3 angularVelocityAxis = angularVelocity.Normalized();
            Basis rotation = new Basis(angularVelocityAxis, angularVelocityMagnitude * PhysicsServerHelper.deltaFromPhysicsProcess);
            meshToRotateTransform.Basis *= rotation;
            meshToRotateTransform = meshToRotateTransform.Orthonormalized();
        }
        return meshToRotateTransform;
    }
}