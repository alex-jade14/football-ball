using Godot;

public partial class CircleHelper{
     public static float CalculateDiameterFromRadius(float radius){
        return radius * 2;
    }

     public static float CalculateRadiusFromDiameter(float diameter){
        return diameter / 2;
    }

    public static float CalculateCircumferenceFromRadius(float radius){
        return 2 * Mathf.Pi * radius;
    }

     public static float CalculateRadiusFromCircumference(float circumference){
        return circumference / (2 * Mathf.Pi);
    }

    public static float CalculateCrossSectionalArea(float radius){
        return Mathf.Pi * Mathf.Pow(radius,2);
    }
}