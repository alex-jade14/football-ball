using Godot;
using System;

public partial class Environment
{
    private float _densityOfFluid {
        get{
            return GetDensityOfFluid();
        }
        set{
            SetDensityOfFluid(value);
        }
    }

    public float GetDensityOfFluid(){
        return _densityOfFluid;
    }

    public void SetDensityOfFluid(float densityOfFluid){
        _densityOfFluid = densityOfFluid;
    }

}