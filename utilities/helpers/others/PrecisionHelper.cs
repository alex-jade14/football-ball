using Godot;
using System;

public partial class PrecisionHelper
{
    public static float ValueWithTruncatedDecimals(float number, int decimalsQuantity){
        int precisionFactor = (int) Math.Pow(10, decimalsQuantity);
        if(precisionFactor > 1){
            return (float) Math.Truncate(number * precisionFactor) / precisionFactor;
        }
        return (float) Math.Truncate(number);
    }

}


