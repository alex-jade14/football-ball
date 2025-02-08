using Godot;

public partial class AerodynamicHelper
{
    public static Vector3[] AerodynamicEffectsOfASphereOnAir(float dragCoefficient, float densityOfFluid, 
    float liftCoefficient, float crossSectionalArea, Vector3 linearVelocity, Vector3 angularVelocity,
    bool canApplyAirResistance, bool canApplyMagnusEffect){
        Vector3 dragForce = new(0, 0, 0);
        if(canApplyAirResistance){
            dragForce = CalculateDragForce(
                dragCoefficient,
                densityOfFluid,
                crossSectionalArea,
                linearVelocity
            );
        }
         
        Vector3 magnusEffectForce = new(0, 0, 0); 
        if(canApplyMagnusEffect){
            magnusEffectForce = CalculateMagnusEffectForce(
                densityOfFluid,
                liftCoefficient,
                crossSectionalArea,
                linearVelocity,
                angularVelocity
            );
        }
    
        return new Vector3[] {dragForce, magnusEffectForce};
    }

    public static Vector3 CalculateDragForce(float dragCoefficient, float densityOfFluid, float crossSectionalArea,
    Vector3 linearVelocity){
        Vector3 dragForce = -(0.5f * dragCoefficient * densityOfFluid * crossSectionalArea * linearVelocity.Length() * linearVelocity);
        return dragForce;
    }

    public static Vector3 CalculateMagnusEffectForce(float densityOfFluid, float liftCoefficient, float crossSectionalArea,
    Vector3 linearVelocity, Vector3 angularVelocity){
        Vector3 magnusEffectForce = 0.5f * densityOfFluid * liftCoefficient * crossSectionalArea * Mathf.Pow(linearVelocity.Length(), 2) * angularVelocity.Normalized().Cross(linearVelocity.Normalized());
        return magnusEffectForce;
    }

    public static float LimitToTerminalVelocity(float terminalVelocity, float yVelocityComponent){
        if (yVelocityComponent < 0 && yVelocityComponent < -terminalVelocity){
            yVelocityComponent = -terminalVelocity;
        }
        return yVelocityComponent;
    }

    public static float CalculateTerminalVelocity(float crossSectionalArea, float mass, float dragCoefficient, float densityOfFluid){
        return Mathf.Sqrt((2 * mass * PhysicsHelper.Gravity) / (densityOfFluid * dragCoefficient * crossSectionalArea));
    }
}