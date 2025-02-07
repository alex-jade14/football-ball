using Godot;

public partial class SphereHelper : CircleHelper{
    public static float CalculateInertiaFromThinSphere(float mass, float radius){
        return 0.66f * mass * Mathf.Pow(radius, 2f);
    }
}