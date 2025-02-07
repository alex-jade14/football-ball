using Godot;

public partial class RigidBodyHelper
{
    public static Vector3 CalculateCentralForces(Vector3[] forcesToApply, float mass, Vector3 linearVelocity){
        Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForcesInVectorForm(
            forcesToApply,
            mass
        );
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, PhysicsServerHelper.DeltaFromPhysicsProcess);
        return linearVelocity;
    }

    public static Vector3 CalculateForces(Vector3[] forcesToApply, Vector3 positionWhereForcesAreApplied, float mass,
    float inertia, Vector3 linearVelocity){
        //Not complete yet
        Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForcesInVectorForm(forcesToApply, mass);
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(linearAcceleration, PhysicsServerHelper.DeltaFromPhysicsProcess);
        return linearVelocity;
    }

    public static Vector3 CalculateCentralImpulse(Vector3 impulse, float mass, Vector3 linearVelocity){
        Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(impulse, mass);
        linearVelocity += linearAcceleration;
        return linearVelocity;
    }

    public static (Vector3 linearVelocity, Vector3 angularVelocity) CalculateImpulse(Vector3 impulse, Vector3 positionWhereImpulseIsApplied, float mass,
    float inertia, Vector3 linearVelocity, Vector3 angularVelocity){
        Vector3 linearAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(impulse, mass);
        linearVelocity += linearAcceleration;
        Vector3 torque = RotationalMotionHelper.CalculateTorque(positionWhereImpulseIsApplied, impulse);
        Vector3 angularAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(torque, inertia);
        angularVelocity += MotionHelper.CalculateVelocityFromAcceleration(angularAcceleration, PhysicsServerHelper.DeltaFromPhysicsProcess);
        return (linearVelocity, angularVelocity);
    }

    public static Vector3 CalculateAngularDamping(float angularDampingCoefficient, Vector3 angularVelocity){
        float angularResult = 1.0f - PhysicsServerHelper.DeltaFromPhysicsProcess * angularDampingCoefficient;
        angularResult = angularResult < 0 ? 0 : angularResult;
        angularVelocity *= angularResult;
        return angularVelocity;
    }

    public static Transform3D CalculateAngularRotation(Vector3 angularVelocity, Transform3D meshToRotateTransform){
        float angularVelocityMagnitude = angularVelocity.Length();
        if(angularVelocityMagnitude > 0){
            Vector3 angularVelocityAxis = angularVelocity.Normalized();
            Basis rotation = new(angularVelocityAxis, angularVelocityMagnitude * PhysicsServerHelper.DeltaFromPhysicsProcess);
            meshToRotateTransform.Basis *= rotation;
            meshToRotateTransform = meshToRotateTransform.Orthonormalized();
        }
        return meshToRotateTransform;
    }
}