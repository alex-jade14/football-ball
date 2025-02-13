using Godot;

public class WorldEnvironment
{
    private float _densityOfFluid;

    public WorldEnvironment(float densityOfFluid){
        SetDensityOfFluid(densityOfFluid);
    }

    public float GetDensityOfFluid(){
        return _densityOfFluid;
    }

    public void SetDensityOfFluid(float densityOfFluid){
        _densityOfFluid = Mathf.Clamp(
            densityOfFluid,
            WorldEnvironmentParametersRanges.MinDensityOfFluidValue,
            WorldEnvironmentParametersRanges.MaxDensityOfFluidValue
        );
    }

}