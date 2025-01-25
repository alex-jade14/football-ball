using Godot;
using System;

public static class UnitsConverterHelper
{
    public static float ConvertMSToKMH(float valueInMS){
        return valueInMS * 3.6f;
    }

    public static float ConvertKMHToMS(float valueInKMH){
        return valueInKMH / 3.6f;
    }

    public static float ConvertGramToKilogram(float valueInGram){
        return valueInGram / 1000f;
    }

    public static float ConvertKilogramToGram(float valueInKilogram){
        return valueInKilogram / 1000f;
    }

    public static float ConvertMetersToCentimeters(float valueInMeters){
        return valueInMeters * 100f;
    }

    public static float ConvertCentimetersToMeters(float valueInCentimeters){
        return valueInCentimeters / 100f;
    }
}
