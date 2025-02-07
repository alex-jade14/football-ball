public partial class NewtonsFirstLawHelper
{
    public static float CalculateNormalForce(float mass){
        return mass * PhysicsHelper.Gravity;
    }
}