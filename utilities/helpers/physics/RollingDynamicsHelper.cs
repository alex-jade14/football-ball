using Godot;
using System;

public partial class RollingDynamicsHelper{
    public static float rotationReductionFactorBox = 5;
    public static float interpolationCoefficient = 0.01f;

    public static (Vector3 linearVelocity, Vector3 angularVelocity) CalculateRollingWithoutSlipping(Vector3 frictionForce, Vector3 upDirection, float mass,
    Vector3 linearVelocity, Vector3 angularVelocity, bool canRoll, float radius, Godot.Vector3 collisionPosition, float inertia, float frictionCoefficient, Godot.Vector3 collisionNormal, float frictionValue){
        Vector3 acceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(frictionForce, mass);
        linearVelocity += MotionHelper.CalculateVelocityFromAcceleration(acceleration, PhysicsServerHelper.deltaFromPhysicsProcess);
        if(canRoll){
            // var slippingSpeed = linearVelocity.Length() - angularVelocity.Length() * radius;
            // if(slippingSpeed > 0.01f){
            //     Vector3 torque = RotationalMotionHelper.CalculateTorque(collisionPosition, frictionForce);
            //     Vector3 angularAcceleration = NewtonsSecondLawHelper.CalculateAccelerationWithForceInVectorForm(torque, inertia);
            //     angularVelocity -= MotionHelper.CalculateVelocityFromAcceleration(angularAcceleration, PhysicsServerHelper.deltaFromPhysicsProcess); 
            // }
            // else{
                
            // }
            var tangentVelocity = linearVelocity - (linearVelocity.Dot(collisionNormal)) * collisionNormal;
            float angularSpeed = linearVelocity.Length() / radius;
            Vector3 rotationAxis = upDirection.Cross(linearVelocity.Normalized()).Normalized();
            Vector3 newAngularVelocity = rotationAxis * angularSpeed;
            angularVelocity = newAngularVelocity;
        }
        else{
            var tangentVelocity = linearVelocity - (linearVelocity.Dot(collisionNormal)) * collisionNormal;
            var auxE = collisionPosition.Cross(angularVelocity) - tangentVelocity;
            var e = auxE / auxE.Length();
            var f = collisionPosition.Cross(e)/radius;
            var Fn = 2 * mass * ((linearVelocity * collisionNormal) / 0.05f);
            var Ft = frictionCoefficient * Fn * e;
            angularVelocity +=  f * ((Ft.Length() * radius) / inertia) * PhysicsServerHelper.deltaFromPhysicsProcess;
        }
        return (linearVelocity, angularVelocity);
    }

}