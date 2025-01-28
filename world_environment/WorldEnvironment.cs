using Godot;
using System;

public class WorldEnvironment
{
    private float _densityOfFluid;

    public WorldEnvironment(float densityOfFluid){
        _densityOfFluid = densityOfFluid;
    }

    public float GetDensityOfFluid(){
        return _densityOfFluid;
    }

    public void SetDensityOfFluid(float densityOfFluid){
        _densityOfFluid = densityOfFluid;
    }

}