using Godot;
using System;
using System.Numerics;
using System.Security.Principal;

public partial class AerodynamicHelper : PhysicsHelper
{
    public static Godot.Vector3[] AerodynamicEffectsOfASphereOnAir(float dragCoefficient, float densityOfFluid, 
    float liftCoefficient, float crossSectionalArea, Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity,
    bool canApplyAirResistance, bool canApplyMagnusEffect){
        Godot.Vector3 dragForce = new Godot.Vector3(0, 0, 0);
        if(canApplyAirResistance){
            dragForce = CalculateDragForce(
                dragCoefficient,
                densityOfFluid,
                crossSectionalArea,
                linearVelocity
            );
        }
         
        Godot.Vector3 magnusEffectForce = new Godot.Vector3(0, 0, 0); 
        if(canApplyMagnusEffect){
            magnusEffectForce = CalculateMagnusEffectForce(
                densityOfFluid,
                liftCoefficient,
                crossSectionalArea,
                linearVelocity,
                angularVelocity
            );
        }
    
        return new Godot.Vector3[] {dragForce, magnusEffectForce};
    }

    public static Godot.Vector3 CalculateDragForce(float dragCoefficient, float densityOfFluid,
    float crossSectionalArea,  Godot.Vector3 linearVelocity){
        Godot.Vector3 dragForce = -(0.5f * dragCoefficient * densityOfFluid * crossSectionalArea * linearVelocity.Length() * linearVelocity);
        return dragForce;
    }

    public static Godot.Vector3 CalculateMagnusEffectForce(float densityOfFluid, float liftCoefficient, float crossSectionalArea,
    Godot.Vector3 linearVelocity, Godot.Vector3 angularVelocity){
        Godot.Vector3 magnusEffectForce = 0.5f * densityOfFluid * liftCoefficient * crossSectionalArea * Mathf.Pow(linearVelocity.Length(), 2) * angularVelocity.Normalized().Cross(linearVelocity.Normalized());
        return magnusEffectForce;
    }

    public static float limitToTerminalVelocity(float terminalVelocity, float yVelocityComponent){
        if (yVelocityComponent < 0 && yVelocityComponent < -terminalVelocity){
            yVelocityComponent = -terminalVelocity;
        }
        return yVelocityComponent;
    }

    public static float CalculateTerminalVelocity(float crossSectionalArea, float mass, float dragCoefficient, float densityOfFluid){
        return Mathf.Sqrt((2 * mass * gravity) / (densityOfFluid * dragCoefficient * crossSectionalArea));
    }

}