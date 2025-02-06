using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class ShadowBall : BallBase, IObserver
{
    public List<Godot.Vector3> positions;
    public  List<Godot.Vector3> velocities;
    private bool _canSimulatePhysics;
    private Drawer _drawer;
    
    public bool _canShowItsTrajectory;
    public Decal _positionMarker;

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

    public Drawer GetDrawer(){
        return _drawer;
    }

    public void SetDrawer(Drawer drawer){
        _drawer = drawer;
    }

    public override void _Ready()
    {
        base._Ready();
        _positionMarker = (Decal) GetNode("PositionMarker");
        ScalePositionMarker();
        _canDetectCollisions = false;
    }

    
    public void UpdateByImpulse(Dictionary data){
        if(data.ContainsKey("impulse") && data.ContainsKey("positionWhereImpulseIsApplied")){
            //ApplyImpulse((Vector3) data["impulse"], (Vector3) data["positionWhereImpulseIsApplied"]);
            EnablePhysicsSimulation();
        }
    }

    public void UpdateByDetectedCollision(Dictionary data){
        if(data.ContainsKey("globalPosition") && data.ContainsKey("linearVelocity") && data.ContainsKey("angularVelocity")){
            SetGlobalPosition((Vector3) data["globalPosition"]);
            SetLinearVelocity((Vector3) data["linearVelocity"]);
            SetAngularVelocity((Vector3) data["angularVelocity"]);
            EnablePhysicsSimulation();
        }
    }

    public void UpdateShadowBallMarker(Dictionary data){
        if(data.ContainsKey("hide")){
            if((bool) data["hide"]){
                _positionMarker.Hide();
            }
        }
    }

    private void EnablePhysicsSimulation(){
        CanSimulatePhysics(true);
        PhysicsSimulation();
    }
    
    private void PhysicsSimulation()
    {
        while(CanSimulatePhysics()){
            Vector3 previousPosition = GetGlobalPosition();
            SimulatePhysics();
            MoveAndSlide();
            if(CanShowItsTrajectory()){
                _drawer.Line(previousPosition, GlobalPosition, Colors.LightGray);
            }
            if(GetGlobalPosition().Y <= GetMeasurement().GetRadius()){
                CanSimulatePhysics(false);
                break;
            }
        }
        ShowPositionMarker();
    }

    public void ShowPositionMarker(){
        _positionMarker.SetGlobalPosition(
            new Vector3(
                GetGlobalPosition().X,
                0,
                GetGlobalPosition().Z
            )
        );
        _positionMarker.Show();
    }

    private void ScalePositionMarker(){
        _positionMarker.SetScale(
            new Vector3(
                GetMeasurement().GetRadius() * 2,
                _positionMarker.GetScale().Y, 
                GetMeasurement().GetRadius() * 2
            )
        );
    }
}