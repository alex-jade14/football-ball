using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class ShadowBall : BallBase, IObserver
{
    public List<Godot.Vector3> positions;
    public  List<Godot.Vector3> velocities;
    private bool _canSimulatePhysics;

    public bool _canShowItsTrajectory;

    public bool CanSimulatePhysics(){
        return _canSimulatePhysics;
    }

    public void CanSimulatePhysics(bool canSimulatePhysics){
        _canSimulatePhysics = canSimulatePhysics;
    }

    public bool CanShowItsTrajectory(){
        return _canShowItsTrajectory;
    }

    public void CanShowItsTrajectory(bool canShowItsTrajectory){
        _canShowItsTrajectory = canShowItsTrajectory;
    }
    
    public void Update(EventManager manager){
        if (manager.state == 1){
            ApplyImpulse((Godot.Vector3) manager.data["impulse"], (Godot.Vector3) manager.data["positionWhereImpulseIsApplied"]);
            CanSimulatePhysics(true);
            PhysicsSimulation();
        }
    }
    
    private void PhysicsSimulation()
    {
        while(CanSimulatePhysics()){
            Vector3 previousPosition = GetGlobalPosition();
            SimulatePhysics();
            MoveAndSlide();
            if(CanShowItsTrajectory()){
                DrawHelper.Line(previousPosition, GlobalPosition, Colors.White);
            }
            if(GetGlobalPosition().Y <= GetMeasurement().GetRadius()){
                CanSimulatePhysics(false);
                break;
            }
        }
    }
}