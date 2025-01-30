using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class ShadowBall : BallBase, IObserver
{
    public List<Godot.Vector3> positions;
    public  List<Godot.Vector3> velocities;
    public bool _canSimulatePhysics;

    public bool CanSimulatePhysics(){
        return _canSimulatePhysics;
    }

    public void CanSimulatePhysics(bool canSimulatePhysics){
        _canSimulatePhysics = canSimulatePhysics;
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
        positions = new List<Godot.Vector3>();
	    velocities = new List<Godot.Vector3>();
        while (CanSimulatePhysics()){
            SimulatePhysics();
            SetGlobalPosition(
                GetGlobalPosition() +
                new Godot.Vector3(
                    GetLinearVelocity().X * PhysicsServerHelper.deltaFromPhysicsProcess,
                    GetLinearVelocity().Y * PhysicsServerHelper.deltaFromPhysicsProcess,
                    GetLinearVelocity().Z * PhysicsServerHelper.deltaFromPhysicsProcess
                )
            );
            positions.Add(GlobalPosition);
            positions.Add(GetLinearVelocity());
            if(GetLinearVelocity().Y < GetMeasurement().GetRadius()){
                CanSimulatePhysics(false);
                break;
            }
        }
    }
}